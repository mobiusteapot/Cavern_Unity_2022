using UnityEngine;

namespace ETC.CaveCavern{
    /// <summary>
    /// Manages if a CavernRig is active or not based on the settings
    /// </summary>
    public abstract class CavernRigActivator : MonoBehaviour
    {
        abstract protected CavernRigType cavernRigType { get; }
        public CavernOutputSettings settings { protected get; set; }
        
        protected virtual void Awake() {
            settings = CavernManager.Instance.Settings;
        }
        protected virtual void Start(){
            SetCavernActive();
        }
        protected bool IsRigTypeEnabled(){
            return settings.rigType == cavernRigType;
        }
        protected virtual void SetCavernActive(){
            gameObject.SetActive(IsRigTypeEnabled());
        }
    }
}