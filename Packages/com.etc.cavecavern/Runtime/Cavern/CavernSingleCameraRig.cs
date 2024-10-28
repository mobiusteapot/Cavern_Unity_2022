using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ETC.CaveCavern {
    public class CavernSingleCameraRig : CavernRigActivator {
        protected override CavernRigType cavernRigType => CavernRigType.SingleCamera;
        [SerializeField] private CavernSingleCameraOutput cavernSingleCameraOutput;
        [SerializeField] private CavernMultiCameraOutput cavernMultiCameraOutput;

        protected override void Start(){
            if(!IsRigTypeEnabled()){
                base.Start();
                return;
            }
            var settings = CavernRenderSettings.Instance;
            if(settings.camOutputMode == CameraOutputMode.SingleDisplay){
                cavernSingleCameraOutput.gameObject.SetActive(true);
                cavernMultiCameraOutput.gameObject.SetActive(false);
            } else if(settings.camOutputMode == CameraOutputMode.MultiDisplay){
                cavernSingleCameraOutput.gameObject.SetActive(false);
                cavernMultiCameraOutput.gameObject.SetActive(true);
            }
        }


    }

}