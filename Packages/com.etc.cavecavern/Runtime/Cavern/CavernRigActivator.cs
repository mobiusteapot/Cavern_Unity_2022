using UnityEngine;

namespace ETC.CaveCavern{
    /// <summary>
    /// Manages if a CavernRig is active or not based on the settings
    /// </summary>
    public abstract class CavernRigActivator : MonoBehaviour
    {
        abstract protected CavernRigType cavernRigType { get; }
        protected virtual void Start(){
            SetCavernActive();
        }
        protected bool IsRigTypeEnabled(){
            return CavernOutputSettings.Instance.rigType == cavernRigType;
        }
        protected virtual void SetCavernActive(){
            gameObject.SetActive(IsRigTypeEnabled());
        }
    }
}