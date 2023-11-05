using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CreateStereoCubemaps : MonoBehaviour
{
    public bool renderStereo = true;
    public float stereoSeparation = 0.064f;
    [SerializeField] private int cubemapMask = 63;
    [SerializeField] private int perEyeWidth = 3840;
    [SerializeField] private int perEyeHeight = 360;
    [SerializeField] private int outputWidth = 3840;
    [SerializeField] private int outputHeight = 720;
    [SerializeField] private CavernOutputCamera outputCamera;
    private RenderTexture cubemapLeftEye;
    private RenderTexture cubemapRightEye;
    private RenderTexture equirect;
    private Camera cubemapCam;
    // todo: add hotkey to swap
    // Re-add debug output
    // Hotkey to set stereo to 0

    void Awake()
    {
        cubemapLeftEye = new RenderTexture(perEyeWidth, perEyeHeight, 24, RenderTextureFormat.ARGB32);
        cubemapLeftEye.dimension = TextureDimension.Cube;
        cubemapRightEye = new RenderTexture(perEyeWidth, perEyeHeight, 24, RenderTextureFormat.ARGB32);
        cubemapRightEye.dimension = TextureDimension.Cube;
        //equirect height should be twice the height of cubemap
        equirect = new RenderTexture(outputWidth, outputHeight, 24, RenderTextureFormat.ARGB32);
        cubemapCam = GetComponent<Camera>();
        cubemapCam.enabled = false;
        if(outputCamera == null)
        {
            Debug.LogWarning("Cavern output camera has not been set, output will not be displayed!");
        }
        else
        {
            outputCamera.SetOutputRT(equirect);
        }
    }

    void LateUpdate()
    {

        if (cubemapCam == null)
        {
            cubemapCam = GetComponentInParent<Camera>();
        }

        if (cubemapCam == null)
        {
            Debug.Log("stereo 360 capture node has no camera or parent camera");
        }

        if (renderStereo)
        {
            cubemapCam.stereoSeparation = stereoSeparation;
            cubemapCam.RenderToCubemap(cubemapLeftEye, cubemapMask, Camera.MonoOrStereoscopicEye.Left);
            cubemapCam.RenderToCubemap(cubemapRightEye, cubemapMask, Camera.MonoOrStereoscopicEye.Right);
        }
        else
        {
            cubemapCam.RenderToCubemap(cubemapLeftEye, cubemapMask, Camera.MonoOrStereoscopicEye.Mono);
        }

        //optional: convert cubemaps to equirect

        if (equirect == null)
            return;

        if (renderStereo)
        {
            cubemapLeftEye.ConvertToEquirect(equirect, Camera.MonoOrStereoscopicEye.Left);
            cubemapRightEye.ConvertToEquirect(equirect, Camera.MonoOrStereoscopicEye.Right);
        }
        else
        {
            cubemapLeftEye.ConvertToEquirect(equirect, Camera.MonoOrStereoscopicEye.Mono);
        }

    }
}
