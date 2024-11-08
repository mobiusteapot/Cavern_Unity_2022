using UnityEngine;

namespace ETC.CaveCavern{
    /// <summary>
    /// Manages if a CavernRig is active or not based on the settings.
    /// </summary>
    public abstract class CavernRigActivator : MonoBehaviour
    {
        abstract protected CavernRigType cavernRigType { get; }
        protected virtual void Start(){
            SetCavernActive();
        }
        /// <summary>
        /// Checks if the Cavern Render Settings specify the rig type of this instance is enabled.
        /// </summary>
        /// <returns>True if it matches the current rig type in settings, false if not a match or settings are not found.</returns>
        protected bool IsRigTypeEnabled(){
            var crs = CavernRenderSettings.Instance;
            if(crs == null){
                Debug.LogError(CavernDebug.NoRenderSettingsFound);
                return false;
            }
            return CavernRenderSettings.Instance.rigType == cavernRigType;
        }
        protected virtual void SetCavernActive(){
            gameObject.SetActive(IsRigTypeEnabled());
        }
    }
}