using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETC.CaveCavern
{
    [DisallowMultipleComponent]
    public class CavernManager : MonoBehaviour
    {
        public static CavernManager Instance;
        [field: SerializeField] public CavernOutputSettings Settings { get; private set; }
        private void Awake()
        {
            Instance = this;
            ValidateOutputManager();
        }

        public bool CurrentlyActiveCheck(Type type)
        {
            switch (Settings.camOutputMode)
            {
                case CameraOutputMode.SingleDisplay:
                    return (type == typeof(CavernSingleCameraOutput));
                case CameraOutputMode.MultiDisplay:
                    return (type == typeof(CavernMultiCameraOutput));
                case CameraOutputMode.MultiDisplayLegacy:
                    return (type == typeof(CavernLegacyRig));
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