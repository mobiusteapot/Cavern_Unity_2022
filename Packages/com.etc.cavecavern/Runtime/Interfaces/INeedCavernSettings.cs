using UnityEngine;

namespace ETC.CaveCavern{
    public interface INeedCavernSettings
    {
        static CavernOutputSettings GetCavernOutputSettings(){
            return CavernManager.Instance.Settings;
        }
        static CavernRigType GetCavernRigType(){
            return GetCavernOutputSettings().rigType;
        }
        static CameraOutputMode GetCameraOutputMode(){
            return GetCavernOutputSettings().camOutputMode;
        }
        static public Rect GetCropRect(){
            return GetCavernOutputSettings().cropRect;
        }
        static public Rect GetStretchRect()
        {
            return GetCavernOutputSettings().stretchRect;
        }
    }
}