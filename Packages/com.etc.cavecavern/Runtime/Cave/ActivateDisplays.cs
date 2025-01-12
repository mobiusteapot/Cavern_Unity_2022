using UnityEngine;

namespace ETC.CaveCavern
{
    /// <summary>
    /// For multi monitor output setups (ie, non-mosaic). Required to activate output for each monitor.
    /// Originally intended for the CAVE
    /// </summary>
    public class ActivateDisplays : MonoBehaviour
    {
        [Tooltip("How many displays are we activating")]
        public int displayCount = 3;

        void Awake()
        {
            for (int i = 1; i < Display.displays.Length && i < displayCount; i++)
            {
                Display.displays[i].Activate();
            }
        }
    }
}