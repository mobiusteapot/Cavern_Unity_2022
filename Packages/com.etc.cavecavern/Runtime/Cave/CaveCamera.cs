
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
        [HideInInspector]
        [SerializeField]
        public RENDER_MODE renderMode;

        [Tooltip("The distance between the pupils in milimeters")]
        public float IPD = 63f;

        [Tooltip("How far from the center of the cavern do we render (for 3D stuff that pops off the screen). 0 renders everything, 1 clips at the screen")]
        public float clipPlane = 1;

        [HideInInspector]
        public Transform headTrackObject; // In head tracked mode, what transform do we bind to?

        [HideInInspector]
        public Vector2Int panelResolution = new Vector2Int(512, 2048); // The resolution of each rendered panel

        // All the cameras used
        private List<OffAxisCam> caveCameras;

        // All the textures the cameras render to
        private List<RenderTexture> cameraOutputTextures;

        public static RenderTexture outFrame { get; private set; }

        // Have our local values been set yet?
        private bool initialized = false;

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
            outFrame = new RenderTexture(panelResolution.x * CavernMultiCameraController.panelCount(), panelResolution.y * 2, 24);
            RenderCam.SetTexture(outFrame);

            // First left, then right eye cameras
            for (int eye = -1; eye <= 1; eye+=2)
            {

                // Create a camera for each plane of our cave controls
                for (int i = 0; i < CavernMultiCameraController.panelCount(); i++)
                {
                    // Get panel position
                    Vector3[] panelCorners = CavernMultiCameraController.getPanelCorners(i);

                    // Camera orientation depends on the mode
                    GameObject cam = new GameObject("Sub Camera");
                    if (renderMode == RENDER_MODE.HEAD_TRACKED)
                    {
                        // Ensure this camera maintains a parent constraint to this camera, but not rotationally
                        CopyTransformPosition posConstraint = cam.AddComponent<CopyTransformPosition>();
                        cam.transform.position = transform.position + transform.right * eye * IPD / 2000f;
                        posConstraint.BindToParent(transform);

                    }
                    else if (renderMode == RENDER_MODE.FLAT)
                        cam.transform.position = transform.position;

                    // Parent under cave for project cleanliness
                    cam.transform.parent = transform.parent;

                    // Rotate to face the plane
                    var rotAmount = (-0.5f * (CavernMultiCameraController.panelCount360() - 3) + i - 1) * 2 * Mathf.PI / CavernMultiCameraController.panelCount360();
                    cam.transform.Rotate(transform.up, rotAmount * Mathf.Rad2Deg, Space.Self);

                    // Move to the side by IPD, keeping tangential to the inner circle
                    if (renderMode == RENDER_MODE.ODS)
                    {
                        float tangentAngle = CavernMultiCameraController.singleton.radius * Mathf.Sin(Mathf.Sqrt(Mathf.Pow(IPD / 2, 2) * Mathf.Pow(CavernMultiCameraController.singleton.radius, 2)));
                        Vector3 tangentPos = Quaternion.AngleAxis(tangentAngle * Mathf.Rad2Deg, cam.transform.up * eye) * cam.transform.forward;
                        cam.transform.position = transform.position + tangentPos.normalized * IPD / 2000f;
                    }


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
            Vector2 scale = new Vector2(CavernMultiCameraController.panelCount(), 2f);

            // First top, then bottom
            for (int i = 0; i < 2; i++)
            {
                // Left to right
                for (int j = 0; j < CavernMultiCameraController.panelCount(); j++)
                {
                    Graphics.CopyTexture(cameraOutputTextures[i * CavernMultiCameraController.panelCount() + j], 0, 0, 0, 0, panelResolution.x, panelResolution.y, outFrame, 0, 0, j * panelResolution.x, i * panelResolution.y);
                }
            }
        }

        void Start()
        {
            // Initiate
            AssembleCameras();
        }

        // Update is called once per frame
        void Update()
        {
            // If we're using head tracking, update our position
            if (renderMode == RENDER_MODE.HEAD_TRACKED && headTrackObject != null) {
                transform.position = headTrackObject.position;
                transform.rotation = headTrackObject.rotation;
            }

            // Make sure the cameras track properly
            foreach (OffAxisCam offCam in caveCameras)
                offCam.UpdateProjectionMatrix();

            // Update our output plane
            PackFrames();
        }
    }
}