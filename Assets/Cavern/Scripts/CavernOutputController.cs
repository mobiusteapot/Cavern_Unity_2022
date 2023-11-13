using ETC.CaveCavern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavernOutputController : MonoBehaviour
{
    public CavernOutputSettings settings { protected get; set; }
    protected virtual void Awake() {
        settings = CavernOutputManager.Instance.Settings;
    }
}
