using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ETC.CaveCavern
{
    // This should probably be inheritance instead
    public class CavernMultiCameraOutput : CavernOutputController {
        protected override CameraOutputMode cameraOutputMode => CameraOutputMode.MultiDisplay;
        protected override CavernRigType cavernRigType => CavernRigType.SingleCamera;
    }
}