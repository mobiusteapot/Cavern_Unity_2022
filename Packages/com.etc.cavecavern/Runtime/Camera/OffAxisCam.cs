using UnityEngine;

namespace ETC.CaveCavern
{
    [RequireComponent(typeof(Camera))]
    public class OffAxisCam : MonoBehaviour
    {
        // The corners of the projection plane
        // [Top Left, Top Right, Bottom Left, Bottom Right]
        private Vector3[] projectionCorners;

        // Properties of the plane (right vector, up vector, normal vector, projection points arranged)
        private Vector3 planeRight, planeUp, planeNormal;
        private Matrix4x4 planeProjectionPoints;

        // Have the local vars been set?
        private bool initialized = false;

        // Component imports
        private Camera cam;

        [HideInInspector]
        public int CavePlaneIDX = -1;

        private float clipPlane;

        /// <summary>
        /// Sets the camera corners in world space for off-axis projection
        /// </summary>
        /// <param name="corners">[Top Left, Top Right, Bottom Left, Bottom Right]</param>
        /// <param name="clipPlane">How far from the center of the cavern do we render (for 3D stuff that pops off the screen). 0 renders everything, 1 clips at the screen</param>
        public void AssignProjectionCorners(Vector3[] cornersUnscaled, float clipPlane = 1)
        {
            this.clipPlane = clipPlane;
            
            if (cornersUnscaled.Length != 4)
                Debug.LogError("4 corners not assigned in AssignProjectionCorners function.");
            else
            {
                // Apply clipping plane offset
                Vector3[] corners = new Vector3[4];
                for (int i = 0; i < 4; i++)
                    corners[i] = transform.position + (cornersUnscaled[i] - transform.position) * clipPlane;

                // Check if there's any differences
                bool different = (projectionCorners == null);
                for (int i = 0; (i < 4) && (!different); i++)
                {
                    if (corners[i] != projectionCorners[i])
                        different = true;
                }

                if (different)
                {
                    projectionCorners = corners;
                    UpdatePlaneValues();
                }
            }
        }

        /// <summary>
        /// Updates the values associated with the lookat plane
        /// </summary>
        private void UpdatePlaneValues()
        {
            Matrix4x4 pPoints = Matrix4x4.zero;

            // Get direction vectors
            planeRight = (projectionCorners[3] - projectionCorners[2]).normalized;
            planeUp = (projectionCorners[0] - projectionCorners[2]).normalized;
            planeNormal = -Vector3.Cross(planeRight, planeUp).normalized;

            // Fill matrix
            pPoints[0, 0] = planeRight.x;
            pPoints[0, 1] = planeRight.y;
            pPoints[0, 2] = planeRight.z;

            pPoints[1, 0] = planeUp.x;
            pPoints[1, 1] = planeUp.y;
            pPoints[1, 2] = planeUp.z;

            pPoints[2, 0] = planeNormal.x;
            pPoints[2, 1] = planeNormal.y;
            pPoints[2, 2] = planeNormal.z;

            pPoints[3, 3] = 1.0f;

            planeProjectionPoints = pPoints;
        }

        /// <summary>
        /// Updates the attached camera's projection matrix so it's looking in the correct direction
        /// </summary>
        /// https://medium.com/try-creative-tech/off-axis-projection-in-unity-1572d826541e
        public void UpdateProjectionMatrix()
        {
            // Make sure we've had our values initialized
            if (!initialized)
                Initialize();
            
            // Get vectors from eye to projection screen corners and center
            Vector3[] pointToCorners = new Vector3[4];
            Vector3 lookatDir = Vector3.zero;
            for (int i = 0; i < 4; i++)
            {
                if (CavePlaneIDX != -1)
                    projectionCorners[i] = CavernMultiCameraController.getPanelCorners(CavePlaneIDX)[i];
                pointToCorners[i] = (projectionCorners[i] - transform.position) * clipPlane;

                lookatDir += pointToCorners[i];
            }

            // Face the center of the panel, but only on the xz plane
            //lookatDir = new Vector3(lookatDir.x, 0, lookatDir.z);
            //cam.transform.forward = -lookatDir.normalized;

            // Get distance from eye to projection plane to clamp near plane (so nothing between is visible)
            float d = -Vector3.Dot(pointToCorners[2], planeNormal);
            cam.nearClipPlane = d;

            // Construct camera frustum
            Matrix4x4 P = Matrix4x4.Frustum(
                Vector3.Dot(planeRight, pointToCorners[2]),
                Vector3.Dot(planeRight, pointToCorners[3]),
                Vector3.Dot(planeUp, pointToCorners[2]),
                Vector3.Dot(planeUp, pointToCorners[0]),
                cam.nearClipPlane,
                cam.farClipPlane);

            // Translate to eye position
            Matrix4x4 T = Matrix4x4.Translate(-transform.position);
            Matrix4x4 R = Matrix4x4.Rotate(Quaternion.Inverse(transform.rotation) * Quaternion.LookRotation(planeNormal,planeUp));

            // Assign projection matrix values
            cam.worldToCameraMatrix = planeProjectionPoints * R * T;
            cam.projectionMatrix = P;

        }

        // Start is called before the first frame update
        void Awake()
        {
            if (!initialized)
                Initialize();
        }

        /// <summary>
        /// Assigns local vars
        /// </summary>
        private void Initialize()
        {
            // Only do this once
            if (initialized)
                return;
            initialized = true;

            // Component imports
            cam = GetComponent<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            // Ensure the camera's facing the correct direction
            UpdateProjectionMatrix();
        }


        public Color gizmoCol = Color.red;
        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoCol;

            Gizmos.DrawRay(transform.position, projectionCorners[0] - transform.position);
            Gizmos.DrawRay(transform.position, projectionCorners[1] - transform.position);
            Gizmos.DrawRay(transform.position, projectionCorners[2] - transform.position);
            Gizmos.DrawRay(transform.position, projectionCorners[3] - transform.position);

            Gizmos.DrawSphere(projectionCorners[0], 0.03f);
            Gizmos.DrawSphere(projectionCorners[1], 0.03f);
            Gizmos.DrawSphere(projectionCorners[2], 0.03f);
            Gizmos.DrawSphere(projectionCorners[3], 0.03f);

        }
    }
}