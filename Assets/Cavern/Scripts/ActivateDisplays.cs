using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDisplays : MonoBehaviour
{
    [Tooltip("How many displays are we activating")]
    public int displayCount = 3;

    void Awake()
    {
        for (int i = 1; i < Display.displays.Length && i < displayCount; i++)
        {
            Display.displays[i].Activate();
        }
    }
}
