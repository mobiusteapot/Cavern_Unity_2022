using UnityEngine;

namespace ETC.CaveCavern{
    /// <summary>
    /// Manages if a CavernRig is active or not based on the current settings.
    /// </summary>
    public class CavernRigActivator : MonoBehaviour
    {
        protected virtual void Awake(){
            CavernRig[] cavernRigs = FindObjectsByType<CavernRig>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach(CavernRig ca in cavernRigs)
            {
                ca.UpdateIfRigActive();
            }
        }
    }
}