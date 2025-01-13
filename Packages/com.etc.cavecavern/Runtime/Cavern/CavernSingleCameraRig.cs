using UnityEngine;

namespace ETC.CaveCavern {
    public class CavernSingleCameraRig : CavernRig {
        public override CavernRigType CavernRigType => CavernRigType.SingleCamera;
        // Todo: Move this initialization behaviour to the new CavernRigActivator
        [SerializeField] private CavernSingleCameraOutput cavernSingleCameraOutput;
        [SerializeField] private CavernMultiCameraOutput cavernMultiCameraOutput;

        protected void Awake(){
            if(!IsRigTypeEnabled()){
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