using UnityEngine;
using System.Collections;

public abstract class ManualFireBuilding : Building
{

    protected enum fireState { CHARGING, READY, TARGETTING, FIRING };
    protected fireState mFireState = fireState.READY;
    public float chargeTime;
    public bool ready = false;
    protected float currCharge = 0;
    // set up layer mask that excludes all normally excluded layers, as well as layer 8, the "ignore unit raycasts" layer
    protected static int ignoreUnitClickLayerMask = 1 << 8;
    protected static int manualFireBuildingTargettingLayerMask = Physics.DefaultRaycastLayers ^ ignoreUnitClickLayerMask;

    public GameObject TargetReticule;
    protected GameObject mTargetter;

    protected override void Update()
    {
        base.Update();
        if (mFireState == fireState.CHARGING)
        {
            currCharge += Time.deltaTime;
            if (currCharge >= chargeTime)
            {
                GetComponent<MeshRenderer>().material.color = Color.green;
                currCharge = 0;
                mFireState = fireState.READY;
            }
        }
        else if (mFireState == fireState.TARGETTING && mTargetter)
        {
            Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));

            RaycastHit hit;
            if (Physics.Raycast(mousePosWorld, new Vector3(0, -1, 0), out hit, Mathf.Infinity, manualFireBuildingTargettingLayerMask))
                mTargetter.transform.position = hit.point;

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit1;

                if (Physics.Raycast(mouseRay, out hit1))
                {
                    GameObject.Destroy(mTargetter);
                    Camera.main.GetComponent<CommanderBuildingControl>().ableToBuild = true;
                    Fire(hit);
                    mFireState = fireState.CHARGING;
                }
            }
        }
    }

    public abstract void TryLaunch();
    public abstract void Fire(RaycastHit hit);
}
