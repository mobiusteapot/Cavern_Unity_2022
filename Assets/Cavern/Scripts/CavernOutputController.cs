using UnityEngine;

namespace ETC.CaveCavern{
    public abstract class CavernOutputController : MonoBehaviour
    {
        abstract protected CameraOutputMode cameraOutputMode { get; }
        abstract protected CavernRigType cavernRigType { get; }
        public CavernOutputSettings settings { protected get; set; }
        
        protected virtual void Awake() {
            settings = CavernManager.Instance.Settings;
        }
        protected virtual void Start(){
            bool isActive = IsCameraOutputModeEnabled() && IsRigTypeEnabled();
            gameObject.SetActive(isActive);
        }
        protected bool IsRigTypeEnabled(){
            return settings.rigType == cavernRigType;
        }
        protected bool IsCameraOutputModeEnabled(){
            return settings.camOutputMode == cameraOutputMode;
        }
        protected virtual void SetCavernActive(bool isActive){
            gameObject.SetActive(isActive);
        }
    }
}