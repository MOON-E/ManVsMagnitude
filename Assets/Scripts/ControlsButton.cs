using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControlsButton : MonoBehaviour {

    public RawImage controls;

    public void Toggle()
    {
        Debug.Log("toggle");
        if (controls.enabled) controls.enabled = false;
        else controls.enabled = true;
    }
}
