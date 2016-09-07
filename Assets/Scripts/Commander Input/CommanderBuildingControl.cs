using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

// Script that handles player placing buildings
/* Pseudocode:
 * If building selected for creation && no current building ghost:
 *      create building ghost
 * If building ghost can be built where it is & LMB down, start building it:
 * If RMB down: delete buiding ghost - done
 */

public class CommanderBuildingControl : MonoBehaviour {
    public static Dictionary<KeyCode, string> BuildHotkeyToBuildingMap;
    public static Dictionary<string, GameObject> NameToBuildingMap;

    public bool ableToBuild = true;

	public readonly float barrierCooldownTime;
    private float barrierProductionCharge;
    bool barrierReady = true;
    public readonly float pylonCooldownTime;
    private float pylonProductionCharge;
    bool pylonReady = true;
    public readonly float factoryCooldownTime;
    private float factoryProductionCharge;
    bool factoryReady = true;
    public readonly float missileTowerCooldownTime;
    private float missileTowerProductionCharge;
    bool missileTowerReady = true;
    public readonly float mineCooldownTime;
    private float mineProductionCharge;
    bool mineReady = true;
    public float resourceBuildingCooldownTime;
    private float resourceBuildingProductionCharge;
    bool resourceBuildingReady = true;

    public uint resourceCount;

    public Button[] buttons;

    Building mBuildingGhost = null; // reference to "ghost" building you plan to build
    List<Building> mBuildings = new List<Building>();

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        UpdateBuildingCooldowns();

        //update GUI based on cooldowns
        buttons[0].interactable = barrierReady && ableToBuild;
        buttons[1].interactable = pylonReady && ableToBuild;
        buttons[2].interactable = factoryReady && ableToBuild;
        buttons[3].interactable = missileTowerReady && ableToBuild;
        buttons[4].interactable = resourceBuildingReady && ableToBuild;

        //update building ghost location
        if (mBuildingGhost)
        {
            Renderer rend = mBuildingGhost.GetComponent<Renderer>();
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            GridNode hoverNode = null;
            RaycastHit hit;

            if (Physics.Raycast(mouseRay, out hit))
            {
                hoverNode = hit.transform.GetComponent<GridNode>();
                if (hoverNode != null)
                {
                    mBuildingGhost.transform.position = hoverNode.transform.position;
                }
            }

            if ((hoverNode != null) && hoverNode.CanBuildHere())
            {
                rend.material.color = Color.green;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    mBuildingGhost.StartBuild(hoverNode);
                    mBuildings.Add(mBuildingGhost);
                    hoverNode.Build(mBuildingGhost);
                    mBuildingGhost = null;
                }
            }
            else rend.material.color = Color.red;

        }

        if (ableToBuild)
            GetBuildingControlInput();
    }

    private void UpdateBuildingCooldowns()
    {
        if (!barrierReady)
        {
            barrierProductionCharge += Time.deltaTime;
            if (barrierProductionCharge >= barrierCooldownTime)
            {
                barrierReady = true;
                barrierProductionCharge = 0;
            }
        }
        if (!pylonReady)
        {
            pylonProductionCharge += Time.deltaTime;
            if (pylonProductionCharge >= pylonCooldownTime)
            {
                pylonReady = true;
                pylonProductionCharge = 0;
            }
        }
        if (!factoryReady)
        {
            factoryProductionCharge += Time.deltaTime;
            if (factoryProductionCharge >= factoryCooldownTime)
            {
                factoryReady = true;
                factoryProductionCharge = 0;
            }
        }
        if (!missileTowerReady)
        {
            missileTowerProductionCharge += Time.deltaTime;
            if (missileTowerProductionCharge >= missileTowerCooldownTime)
            {
                missileTowerReady = true;
                missileTowerProductionCharge = 0;
            }
        }
        if (!resourceBuildingReady)
        {
            resourceBuildingProductionCharge += Time.deltaTime;
            if (resourceBuildingProductionCharge >= resourceBuildingCooldownTime)
            {
                resourceBuildingReady = true;
                resourceBuildingProductionCharge = 0;
            }
        }
        if (!mineReady)
        {
            mineProductionCharge += Time.deltaTime;
            if (mineProductionCharge >= mineCooldownTime)
            {
                mineReady = true;
                mineProductionCharge = 0;
            }
        }
    }

    private void GetBuildingControlInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
            UnqueueBuilding();
        if (Input.GetKeyDown(KeyCode.B) && barrierReady)
        {
            if (mBuildingGhost != null)
                UnqueueBuilding();
            QueueBarrier();
        }
        if (Input.GetKeyDown(KeyCode.P) && pylonReady)
        {
            if (mBuildingGhost != null)
                UnqueueBuilding();
            QueuePylon();
        }
        if (Input.GetKeyDown(KeyCode.M) && missileTowerReady)
        {
            if (mBuildingGhost != null)
                UnqueueBuilding();
            QueueLaser();
        }
        if (Input.GetKeyDown(KeyCode.F) && factoryReady)
        {
            if (mBuildingGhost != null)
                UnqueueBuilding();
            QueueFactory();
        }
        if (Input.GetKeyDown(KeyCode.R) && resourceBuildingReady)
        {
            if (mBuildingGhost != null)
                UnqueueBuilding();
            QueueResourceBuilding();
        }
        if (Input.GetKeyDown(KeyCode.X) && mineReady)
        {
            if (mBuildingGhost != null)
                UnqueueBuilding();
            QueueMine();
        }
    }

    public void DisableBuilding()
    {
        UnqueueBuilding();
        ableToBuild = false;
    }

    public void EnableBuilding()
    {
        ableToBuild = true;
    }

    public void UnqueueBuilding() {
        if (mBuildingGhost != null) {
            DestroyObject(mBuildingGhost.gameObject);
            mBuildingGhost = null;
        }
    }

    public void QueueBarrier() {
        if (!barrierReady) return;
        Debug.Log("making barrier");
        UnqueueBuilding();
        GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/Barrier"), new Vector3(0, 0, 0), Quaternion.identity);
        mBuildingGhost = obj.GetComponent<Building>();
        barrierReady = false;
    }
    public void QueuePylon() {
        if (!pylonReady) return;
        Debug.Log("making pylon");
        UnqueueBuilding();
        GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/Pylon"), new Vector3(0, 0, 0), Quaternion.identity);
        mBuildingGhost = obj.GetComponent<Building>();
        pylonReady = false;
    }
    public void QueueLaser() {
        if (!missileTowerReady) return;
        Debug.Log("making missile");
        UnqueueBuilding();
        GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/Missile"), new Vector3(0, 0, 0), Quaternion.identity);
        mBuildingGhost = obj.GetComponent<Building>();
        missileTowerReady = false;
    }
    public void QueueFactory() {
        if (!factoryReady) return;
        Debug.Log("making factory");
        UnqueueBuilding();
        GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/Factory"), new Vector3(0, 0, 0), Quaternion.identity);
        mBuildingGhost = obj.GetComponent<Building>();
        factoryReady = false;
    }

    public void QueueMine()
    {
        if (!mineReady) return;
        Debug.Log("Building Mine");
        UnqueueBuilding();
        GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/Mine"), new Vector3(0, 0, 0), Quaternion.identity);
        mBuildingGhost = obj.GetComponent<Building>();
        mineReady = false;
    }
    
    public void QueueResourceBuilding()
    {
        if (!resourceBuildingReady) return;
        Debug.Log("making resource building");
        UnqueueBuilding();
        GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/ResourceBuilding"), new Vector3(0, 0, 0), Quaternion.identity);
        obj.GetComponent<ResourceBuilding>().commanderRef = this;
        mBuildingGhost = obj.GetComponent<Building>();
        resourceBuildingReady = false;
    }
}
