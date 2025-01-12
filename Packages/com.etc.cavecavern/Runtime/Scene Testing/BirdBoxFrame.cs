using System;
using UnityEngine;

namespace ETC.CaveCavern
{
    /// <summary>
    /// Lightweight component to hold the transform as a possible position for the bird box.
    /// </summary>
    [Serializable]
    public class BirdBoxFrame : MonoBehaviour
    {
        public BirdBoxMode mode;
    }
}