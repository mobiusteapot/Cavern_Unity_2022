using UnityEngine;

namespace ETC.CaveCavern
{
    /// <summary>
    /// Utility class for throwing common debug messages
    /// </summary>
    public static class CavernDebug
    {
        public const string NoRenderSettingsFound = "CavernRenderSettings not found. "
                    + "You can create a CavernRenderSettings asset in your asset browser, or via the Cavern section in project settings.";
    }
}
