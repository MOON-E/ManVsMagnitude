using UnityEngine;
using System.Collections;
using System;

public class Railgun : ManualFireBuilding {

    public override void Launch()
    {
        if (mFireState != fireState.READY) return;

        //Camera.main.GetComponent<CommanderBuildingControl>().MissileLaunch();

        Camera.main.GetComponent<CommanderBuildingControl>().ableToBuild = false;
        //currentMissileTarget = (GameObject)Instantiate(MissileBlastArea, new Vector3(), Quaternion.identity);

        Debug.Log("railgun click");
        GetComponent<MeshRenderer>().material.color = Color.red;
        mFireState = fireState.CHARGING;
    }
}
