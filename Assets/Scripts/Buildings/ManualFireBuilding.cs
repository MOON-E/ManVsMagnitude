using UnityEngine;
using System.Collections;

public abstract class ManualFireBuilding : Building
{

    protected enum fireState { CHARGING, READY, TARGETTING };
    protected fireState mFireState = fireState.READY;
    public float chargeTime;
    public bool ready = false;
    private float currCharge = 0;
    // set up layer mask that excludes all normally excluded layers, as well as layer 8, the "ignore unit raycasts" layer
    protected static int ignoreUnitClickLayerMask = 1 << 8;
    protected static int manualFireBuildingTargettingLayerMask = Physics.DefaultRaycastLayers ^ ignoreUnitClickLayerMask;

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
    }

    public abstract void Launch();
}
