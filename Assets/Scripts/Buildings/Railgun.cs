using UnityEngine;
using System.Collections;
using System;

public class Railgun : ManualFireBuilding {

    public GameObject RailgunBlast;

    protected float mFireCharge = 0;
    public float timeToFire = 20;
    private RaycastHit mHit;

    protected override void Update()
    {
        base.Update();

        if (mFireState == fireState.FIRING)
        {
            mFireCharge += Time.deltaTime;

            //do any visual charge effects

            if (mFireCharge > timeToFire)
            {
                mFireState = fireState.CHARGING;
                mFireCharge = 0;
                //instantiate prefab
            }
        }
    }

    public override void TryLaunch()
    {
        if (mFireState != fireState.READY) return;

        //Camera.main.GetComponent<CommanderBuildingControl>().MissileTryLaunch();

        Camera.main.GetComponent<CommanderBuildingControl>().ableToBuild = false;
        mTargetter = (GameObject)Instantiate(TargetReticule, new Vector3(), Quaternion.identity);
        mFireState = fireState.TARGETTING;

        Debug.Log("railgun click");
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public override void Fire(RaycastHit hit)
    {
        mHit = hit;
        mFireState = fireState.FIRING;
        //currently hardcodes the 
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "Railgun Origin")
            {
                Debug.DrawLine(child.position, new Vector3(hit.point.x, child.position.y, hit.point.z), Color.yellow, timeToFire);
                //draw "tracer" line
                GetComponent<MeshRenderer>().material.color = Color.yellow;
            }
        }
        
    }
}
