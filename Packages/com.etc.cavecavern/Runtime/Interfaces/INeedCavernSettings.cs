using UnityEngine;

namespace ETC.CaveCavern{
    // Todo: Probably depricate this interface
    public interface INeedCavernSettings
    {
        static CavernRigType GetCavernRigType(){
            return CavernRenderSettings.Instance.rigType;
        }
        static CameraOutputMode GetCameraOutputMode(){
            return CavernRenderSettings.Instance.camOutputMode;
        }
        static public Rect GetCropRect(){
            return CavernRenderSettings.Instance.cropRect;
        }
        static public Rect GetStretchRect()
        {
            return CavernRenderSettings.Instance.stretchRect;
        }
    }
}