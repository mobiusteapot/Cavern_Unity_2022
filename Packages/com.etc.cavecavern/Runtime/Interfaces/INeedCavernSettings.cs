using UnityEngine;

namespace ETC.CaveCavern{
    public interface INeedCavernSettings
    {
        static CavernRigType GetCavernRigType(){
            return CavernOutputSettings.Instance.rigType;
        }
        static CameraOutputMode GetCameraOutputMode(){
            return CavernOutputSettings.Instance.camOutputMode;
        }
        static public Rect GetCropRect(){
            return CavernOutputSettings.Instance.cropRect;
        }
        static public Rect GetStretchRect()
        {
            return CavernOutputSettings.Instance.stretchRect;
        }
    }
}