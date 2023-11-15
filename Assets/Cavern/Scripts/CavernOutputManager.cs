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
    public class CavernManager : Singleton<CavernManager>
    {
        [field: SerializeField] public CavernOutputSettings Settings { get; private set; }
        private void Awake()
        {
            ValidateOutputManager();
        }

        private void ValidateOutputManager()
        {
            if (Settings == null)
                Debug.LogWarning("Cavern settings asset is missing. Please assign an output settings asset");
        }
    }

}