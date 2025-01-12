using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomDebug : MonoBehaviour
{
    public static CustomDebug singleton {get; private set;}

    // Start is called before the first frame update
    void Awake()
    {
        if (!singleton)
            singleton = this;
    }

    public static void SetText(string text){
        singleton.GetComponent<Text>().text = text;
    }
}
