using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETC.CaveCavern
{
    public class CavernOutputManager : MonoBehaviour
    {

        [field: SerializeField] public CavernOutputSettings Settings { get; private set; }
        [SerializeField] private CavernSingleCameraOutput singleCam;
        [SerializeField] private CavernMultiCameraOutput multiCam;

        private void Awake()
        {
            ValidateOutputManager();
            singleCam.settings = multiCam.settings = Settings;
            switch (Settings.camOutputMode)
            {
                case CameraOutputMode.SingleDisplay:
                    multiCam.gameObject.SetActive(false); break;
                case CameraOutputMode.MultiDisplay:
                    singleCam.gameObject.SetActive(false); break;

            }
            

        }

        private void ValidateOutputManager()
        {
            if (Settings == null)
                Debug.LogWarning("Cavern settings asset is missing. Please assign an output settings asset");
            if (singleCam == null)
                Debug.LogError("Cavern Output Manager is missing single Camera! Please assign this in the inspector");
            if (multiCam == null)
                Debug.LogError("Cavern Output Manager is missing a reference to the multiCamera component! Please assign this in the inspector");
        }
    }

}