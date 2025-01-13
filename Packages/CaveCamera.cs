
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETC.CaveCavern
{
    [RequireComponent(typeof(Camera))]
    public class CaveCamera : MonoBehaviour
    {
        // Consts and enums
        [System.Serializable]
        public enum RENDER_MODE { HEAD_TRACKED, ODS, FLAT}

        // Public variables
        
        [SerializeField]
        public RENDER_MODE renderMode;
        [SerializeField]  // This allows q to be editable in the Inspector
        public int q;

        [Tooltip("The distance between the pupils in milimeters")]
        public float IPD = 63f;

        [Tooltip("How far from the center of the cavern do we render (for 3D stuff that pops off the screen). 0 renders everything, 1 clips at the screen")]
        public float clipPlane = 1;

        public Transform headTrackObject; // In head tracked mode, what transform do we bind to?public Transform headTrackObject; 
        public Transform headTrackObject2;

        public Vector2Int panelResolution = new Vector2Int(512, 2048); // The resolution of each rendered panel

        // All the cameras used
        private List<OffAxisCam> caveCameras;

        // All the textures the cameras render to
        private List<RenderTexture> cameraOutputTextures;

        public static RenderTexture outFrame { get; private set; }

        // Have our local values been set yet?
        private bool initialized = false;
        private Transform transform1;
        void Awake()
        {
            // Init local vars
            if (!initialized)
                Initialize();
        }

        private void Initialize()
        {
            // Only ever run once
            if (initialized)
                return;
            initialized = true;

            // Set instance vars
            caveCameras = new List<OffAxisCam>();
            cameraOutputTextures = new List<RenderTexture>();
        }

        private void DeleteCameras()
        {
            // Necesarry for edit mode execution
            if (caveCameras == null)
                return;

            // Remove every camera in our list
            for (int i = caveCameras.Count - 1; i >= 0; i--)
            {
                DestroyImmediate(caveCameras[i].gameObject);
                Destroy(cameraOutputTextures[i]);
            }
            caveCameras = new List<OffAxisCam>();
            cameraOutputTextures = new List<RenderTexture>();
        }

        public void AssembleCameras()
        {
            // Clear all existing cameras
            DeleteCameras();

            // Setup our output frame
            outFrame = new RenderTexture(panelResolution.x * CavernLegacyController.panelCount(), panelResolution.y * 2, 24);
            RenderCam.SetTexture(outFrame);

            // First left, then right eye cameras
            for (int eye = -1; eye <= 1; eye += 2)
            {
                // Create a camera for each plane of our cave controls
                for (int i = 0; i < CavernLegacyController.panelCount(); i++)
                {
                    // Get panel position
                    Vector3[] panelCorners = CavernLegacyController.getPanelCorners(i);

                    // Camera orientation depends on the mode
                    GameObject cam = new GameObject("Sub Camera");

                    if (renderMode == RENDER_MODE.HEAD_TRACKED)
                    {
                        // Ensure this camera maintains a parent constraint to this camera, but not rotationally
                        CopyTransformPosition posConstraint = cam.AddComponent<CopyTransformPosition>();

                        // Use headTrackObject for the first half of the cameras
                        if (i < CavernLegacyController.panelCount() / 2)
                        {
                            cam.transform.position = transform.position + transform.right * eye * IPD / 2000f;
                            posConstraint.BindToParent(transform);  // Bind the first set to the main transform
                        }
                        // Use headTrackObject2 (transform1) for the second half of the cameras
                        else if (headTrackObject2 != null && transform1 != null)
                        {
                            cam.transform.position = transform1.position + transform1.right * eye * IPD / 2000f;
                            posConstraint.BindToParent(transform1);  // Bind the second set to transform1
                        }
                    }
                    else if (renderMode == RENDER_MODE.FLAT)
                    {
                        // Use transform for the first half and transform1 for the second half
                        if (i < CavernLegacyController.panelCount())
                            cam.transform.position = transform.position;
                        else if (headTrackObject2 != null && transform1 != null)
                            cam.transform.position = transform1.position;
                    }

                    // Parent under cave for project cleanliness
                    cam.transform.parent = transform.parent;

                    // Rotate to face the plane
                    var rotAmount = (-0.5f * (CavernLegacyController.panelCount360() - 3) + i - 1) * 2 * Mathf.PI / CavernLegacyController.panelCount360();
                    cam.transform.Rotate(transform.up, rotAmount * Mathf.Rad2Deg, Space.Self);

                    // Set up off-axis projection
                    OffAxisCam camProjection = cam.AddComponent<OffAxisCam>();
                    caveCameras.Add(camProjection);
                    camProjection.AssignProjectionCorners(panelCorners, clipPlane);
                    camProjection.CavePlaneIDX = i;
                    camProjection.gizmoCol = eye == -1 ? Color.red : Color.blue;

                    // Set up output textures for this camera
                    RenderTexture camOut = new RenderTexture(panelResolution.x, panelResolution.y, 24);
                    cam.GetComponent<Camera>().targetTexture = camOut;
                    cam.GetComponent<Camera>().cullingMask = GetComponent<Camera>().cullingMask;
                    cam.GetComponent<Camera>().stereoTargetEye = GetComponent<Camera>().stereoTargetEye;
                    cameraOutputTextures.Add(camOut);
                }
            }
        }


        private void PackFrames()
        {
            if (outFrame == null)
                return;

            // Vector2 scale = new Vector2(1f/CaveControls.panelCount360(), 0.5f);
            // Vector2 scale = new Vector2(0.1f, 0.1f);
            Vector2 scale = new Vector2(CavernLegacyController.panelCount(), 2f);

            // First top, then bottom
            for (int i = 0; i < 2; i++)
            {
                // Left to right
                for (int j = 0; j < CavernLegacyController.panelCount() / 2; j++)
                {
                    Graphics.CopyTexture(cameraOutputTextures[i * CavernLegacyController.panelCount() + j], 0, 0, 0, 0, panelResolution.x, panelResolution.y, outFrame, 0, 0, j * panelResolution.x, i * panelResolution.y);
                }
                for (int j = (CavernLegacyController.panelCount() / 2); j < CavernLegacyController.panelCount(); j++)
                {
                    //for this part, add the other half of the cameras using transform1
                    Graphics.CopyTexture(cameraOutputTextures[i * CavernLegacyController.panelCount() + j], 0, 0, 0, 0, panelResolution.x, panelResolution.y, outFrame, 0, 0, j * panelResolution.x, i * panelResolution.y);
                }
            }
        }

        void Start()
        {
            // Initialize transform1 as a new Transform (by creating a new GameObject to hold it, but not adding it to the scene)
            GameObject tempObject = new GameObject(); // Create a temporary object to get its transform
            transform1 = tempObject.transform;        // Assign transform1 to the temporary object's transform

            AssembleCameras();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            // If we're using head tracking, update our position
            if (renderMode == RENDER_MODE.HEAD_TRACKED && headTrackObject != null)
            {
                transform.position = headTrackObject.position;
                transform.rotation = headTrackObject.rotation;

                if (headTrackObject2 != null)
                {
                    transform1.position = headTrackObject2.position;
                    transform1.rotation = headTrackObject2.rotation;

                    // Calculate the distance between the two transforms
                    float distance = Vector3.Distance(transform1.position, transform.position);

                    // Calculate the angle difference between the two rotations
                    float angleDifference = Quaternion.Angle(transform.rotation, transform1.rotation);

                    // Check if both the distance and angle difference conditions are met
                    if (distance < 1.6f && angleDifference < 90f)
                    {
                        // Average the positions
                        Vector3 averagePosition = (transform1.position + transform.position) / 2;

                        // Average the rotations using Quaternion.Slerp (Smooth interpolation)
                        Quaternion averageRotation = Quaternion.Slerp(transform.rotation, transform1.rotation, 0.5f);

                        // Set both transforms to the average position and rotation
                        transform.position = averagePosition;
                        transform.rotation = averageRotation;

                        transform1.position = averagePosition;
                        transform1.rotation = averageRotation;
                    }
                }
            }
            // Make sure the cameras track properly
            foreach (OffAxisCam offCam in caveCameras)
                offCam.UpdateProjectionMatrix();

            // Update our output plane
            PackFrames();
        }
    }
}