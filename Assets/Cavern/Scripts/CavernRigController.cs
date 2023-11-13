using UnityEngine;

namespace ETC.CaveCavern {
    /// <summary>
    /// Generic high level class for each type of cavern controller
    /// </summary>
    public abstract class CavernRigController : MonoBehaviour
    {
        public CavernOutputSettings settings { protected get; set; }
        protected virtual void Awake() {
            this.ValidateIfCavernTypeEnabled();
            settings = CavernOutputManager.Instance.Settings;
        }
    }

}
