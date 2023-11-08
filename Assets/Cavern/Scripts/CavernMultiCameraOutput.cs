using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ETC.CaveCavern
{
    public class CavernMultiCameraOutput : MonoBehaviour
    {
        private CavernOutputSettings settings;

        private void Awake()
        {
            settings = CavernManager.Instance.Settings;
            this.gameObject.ValidateIfEnabled();
        }
    }
}