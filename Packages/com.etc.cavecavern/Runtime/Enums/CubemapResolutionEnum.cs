using UnityEngine;

namespace ETC.CaveCavern 
{
    public enum CubemapResolution
    {
        /// <summary> 256 x 256 </summary>
        [InspectorName("256 x 256")]
        VeryLow = 256,
        /// <summary> 512 x 512 </summary>
        [InspectorName("512 x 512")]
        Low = 512,
        /// <summary> 1024 x 1024 </summary>
        [InspectorName("1024 x 1024")]
        Medium = 1024,
        /// <summary> 2048 x 2048 </summary>
        [InspectorName("2048 x 2048")]
        High = 2048,
        /// <summary> 4096 x 4096 </summary>
        [InspectorName("4096 x 4096")]
        VeryHigh = 4096,
        /// <summary> 8192 x 8192 </summary>
        [InspectorName("8192 x 8192")]
        Ultra = 8192,
    }
}