using UnityEngine;
using System.Collections;

public class Missile : ManualFireBuilding {

    public GameObject MissileBlastArea;
    private GameObject currentMissileTarget;

    protected override void Update()
    {
        base.Update();

        if (mFireState == fireState.TARGETTING)
        {
            Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));

            RaycastHit hit;
            if (Physics.Raycast(mousePosWorld, new Vector3(0, -1, 0), out hit, Mathf.Infinity, manualFireBuildingTargettingLayerMask))
                currentMissileTarget.transform.position = hit.point;

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit1;

                if (Physics.Raycast(mouseRay, out hit1))
                {
                    Object.Destroy(currentMissileTarget);
                    Camera.main.GetComponent<CommanderBuildingControl>().ableToBuild = true;
                    GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/MissileBlast"), hit.point, Quaternion.identity);
                    mFireState = fireState.CHARGING;
                }
            }
        }
    }

    public override void Launch()
    {
        if (mFireState != fireState.READY) return;
        mFireState = fireState.TARGETTING;

        Camera.main.GetComponent<CommanderBuildingControl>().ableToBuild = false;
        currentMissileTarget = (GameObject)Instantiate(MissileBlastArea, new Vector3(), Quaternion.identity);

        Debug.Log("missile click");
		GetComponent<MeshRenderer>().material.color = Color.red;
    }
}
