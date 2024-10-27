using UnityEngine;

namespace ETC.CaveCavern{
    public interface INeedCavernSettings
    {
        static CavernRigType GetCavernRigType(){
            return CavernOutputSettingsSO.Instance.rigType;
        }
        static CameraOutputMode GetCameraOutputMode(){
            return CavernOutputSettingsSO.Instance.camOutputMode;
        }
        static public Rect GetCropRect(){
            return CavernOutputSettingsSO.Instance.cropRect;
        }
        static public Rect GetStretchRect()
        {
            return CavernOutputSettingsSO.Instance.stretchRect;
        }
    }
}