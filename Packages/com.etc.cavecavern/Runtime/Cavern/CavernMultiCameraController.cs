using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETC.CaveCavern
{
    // Todo: need to refactor CavernOutputController, inheriting this is unintutive
    public class CavernMultiCameraController : CavernRigActivator
    {
        protected override CavernRigType cavernRigType => CavernRigType.MultiCamera;
        [Tooltip("Cave Radius in Meters")]
        public float radius = 3;

        [Tooltip("Cave Height in Meters")]
        public float height = 2;

        [Tooltip("Cave Elevation in Meters")]
        public float elevation = 0.1f;

        [Tooltip("How many planes are we using to approximate this cylinder?")]
        public int resolution = 10;

        [Tooltip("How many degrees does the cylinder revolve?")]
        public float angle = 270;

        // Singleton pattern
        public static CavernMultiCameraController singleton { get; private set; }

        // Have our local values been set yet?
        private bool initialized = false;
        protected void Awake()
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

            // Set the shader parameters
            Shader.SetGlobalInt("_NPanels", resolution);
            Shader.SetGlobalFloat("_Radius", radius);

            // Singleton pattern
            if (!singleton)
                singleton = this;
            else
                Debug.LogError("More than one CaveControls object exists. There should only be one.");
        }

        /// <summary>
        /// Retrieves the 4 corners in 3D space of the panel at the particular index of the resolution
        /// </summary>
        /// <param name="index">The index of the panel (must be lower than resolution)</param>
        /// <returns>[Top Left, Top Right, Bottom Left, Bottom Right] (in world coordinates)</returns>
        private Vector3[] GetPanelCorners(int index)
        {
            Vector3[] corners = new Vector3[4];

            Vector3 blDir = Quaternion.AngleAxis(360 * index / resolution, Vector3.up) * Vector3.forward;
            Vector3 bl = transform.position + blDir.normalized * radius + Vector3.up * elevation;
            Vector3 brDir = Quaternion.AngleAxis(360 * (index+1) / resolution, Vector3.up) * Vector3.forward;
            Vector3 br = transform.position + brDir.normalized * radius + Vector3.up * elevation;

            corners[0] = bl + transform.up * height;
            corners[1] = br + transform.up * height;
            corners[2] = bl;
            corners[3] = br;

            return corners;
        }

        // STATIC METHODS

        /// <summary>
        /// Gets the number of panels this cave would posess if 360 degrees
        /// </summary>
        /// <returns>Number of panels</returns>
        public static int panelCount360()
        {
            return singleton.resolution;
        }

        /// <summary>
        /// Gets the number of panels this cave would posess if 360 degrees
        /// </summary>
        /// <returns>Number of panels</returns>
        public static int panelCount()
        {
            return singleton.resolution * Mathf.FloorToInt(getAngle()) / 360;
        }

        /// <summary>
        /// Gets the internal angle of this cave in degrees
        /// </summary>
        /// <returns></returns>
        public static float getAngle()
        {
            return singleton.angle;
        }

        /// <summary>
        /// Gets the 4 corners of the panel at this index
        /// </summary>
        /// <param name="index">index of a particular panel</param>
        /// <returns>[Top Left, Top Right, Bottom Left, Bottom Right] (In world coordinates)</returns>
        public static Vector3[] getPanelCorners(int index)
        {
            return singleton.GetPanelCorners(index);
        }

        void Update()
        {
            // Necessary for edit mode execution - things aren't always properly initialized
            if (!initialized)
                Initialize();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            for (int i = 0; i < resolution * angle / 360; i++)
            {                
                Vector3[] corners = GetPanelCorners(i);

                for (int j = 0; j < corners.Length; j++)
                {
                    corners[j] = transform.position + transform.rotation * (corners[j] - transform.position);
                }
                Gizmos.DrawLine(corners[0], corners[1]);
                Gizmos.DrawLine(corners[1], corners[3]);
                Gizmos.DrawLine(corners[3], corners[2]);
                Gizmos.DrawLine(corners[2], corners[0]);
            }
        }
    }
}