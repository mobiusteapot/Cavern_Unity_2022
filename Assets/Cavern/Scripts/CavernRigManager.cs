using System;
using UnityEngine;

namespace ETC.CaveCavern
{
    /// <summary>
    /// The Cavern Manager is a singleton that manages the output mode of the Cavern system.
    /// Each Cavern Controller should validate with the Cavern Manager to ensure that the correct output mode is being used.
    /// Each 
    /// </summary>
    [DisallowMultipleComponent]
    public class CavernRigManager : Singleton<CavernRigManager>
    {
        [field: SerializeField] public CavernOutputSettings Settings { get; private set; }
        private void Awake()
        {
            ValidateOutputManager();
        }

        public bool CurrentlyActiveCheck(Type type)
        {
            switch (Settings.rigType)
            {
                case CavernRigType.SingleCamera:
                    switch (Settings.camOutputMode) {
                        case CameraOutputMode.SingleDisplay:
                            return (type == typeof(CavernSingleCameraOutput));
                        case CameraOutputMode.MultiDisplay:
                            return (type == typeof(CavernMultiCameraOutput));
                        default:
                            Debug.LogError("Unknown type attempting to validate with Cavern Manager");
                            return false;
                    }
                case CavernRigType.Legacy:
                    return (type == typeof(CavernLegacyController));
                default:
                    Debug.LogError("Unknown type attempting to validate with Cavern Manager");
                    return false;
            }
        }
        private void ValidateOutputManager()
        {
            if (Settings == null)
                Debug.LogWarning("Cavern settings asset is missing. Please assign an output settings asset");
        }
    }

}