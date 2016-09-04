using UnityEngine;
using System.Collections;
using System;

public class Railgun : ManualFireBuilding {

    public override void Launch()
    {
        if (mFireState != fireState.READY) return;

        //Camera.main.GetComponent<CommanderBuildingControl>().MissileLaunch();

        Debug.Log("missile click");
        GetComponent<MeshRenderer>().material.color = Color.red;
        mFireState = fireState.CHARGING;
    }
}
