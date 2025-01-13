using UnityEngine;

namespace ETC.CaveCavern
{
    /// <summary>
    /// Abstract base class for shared cavern rig behaviour
    /// </summary>
    // Rn only used for activation but could hold more shared logic?
    public abstract class CavernRig : MonoBehaviour
    {
        abstract public CavernRigType CavernRigType { get; }
        /// <summary>
        /// Checks if the Cavern Render Settings specify the rig type of this instance is enabled.
        /// </summary>
        /// <returns>True if it matches the current rig type in settings, false if not a match or settings are not found.</returns>
        public bool IsRigTypeEnabled()
        {
            var crs = CavernRenderSettings.Instance;
            if (crs == null)
            {
                Debug.LogError(CavernDebug.NoRenderSettingsFound);
                return false;
            }
            return CavernRenderSettings.Instance.rigType == CavernRigType;
        }
        /// <summary>
        /// Sets game object to active or unactive based on if it's rig type is enabled.
        /// </summary>
        public virtual void UpdateIfRigActive()
        {
            gameObject.SetActive(IsRigTypeEnabled());
        }
    }
}
