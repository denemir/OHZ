using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHandler : MonoBehaviour
{
    private bool isDebuggingOn = false; //off by default

    //setters and getters
    public void ToggleDebug()
    {
        isDebuggingOn = !isDebuggingOn;
    }
    public bool IsDebuggingOn()
    { return isDebuggingOn; }

}
