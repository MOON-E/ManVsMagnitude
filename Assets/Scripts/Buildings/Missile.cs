using UnityEngine;
using System.Collections;
using System;

public class Missile : ManualFireBuilding {

    protected override void Update()
    {
        base.Update();
    }

    public override void TryLaunch()
    {
        if (mFireState != fireState.READY) return;

        Camera.main.GetComponent<CommanderBuildingControl>().ableToBuild = false;
        mTargetter = (GameObject)Instantiate(TargetReticule, new Vector3(), Quaternion.identity);
        mFireState = fireState.TARGETTING;

        Debug.Log("missile click");
		GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public override void Fire(RaycastHit hit)
    {
        GameObject.Instantiate(Resources.Load("Prefabs/MissileBlast"), hit.point, Quaternion.identity);
    }
}
