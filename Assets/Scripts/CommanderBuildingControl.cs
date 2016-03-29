using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    bool missileTargeting;

    Building mBuildingGhost = null; // reference to "ghost" building you plan to build
    List<Building> mBuildings = new List<Building>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse1)&&!missileTargeting)
			UnqueueBuilding();
        if (Input.GetKeyDown (KeyCode.B)) {
			if (mBuildingGhost != null)
				UnqueueBuilding ();
			QueueBarrier ();
		}
		if (Input.GetKeyDown(KeyCode.P)){
			if (mBuildingGhost != null)
				UnqueueBuilding ();
			QueuePylon();
		}
		if (Input.GetKeyDown (KeyCode.L)) {
			if (mBuildingGhost != null)
				UnqueueBuilding ();
			QueueLaser ();
		}
		if (Input.GetKeyDown(KeyCode.F)){
			if (mBuildingGhost != null)
				UnqueueBuilding ();
			QueueFactory();
		}

        if (mBuildingGhost)
        {
            Renderer rend = mBuildingGhost.GetComponent<Renderer>();
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            GridNode hoverNode = null;
            RaycastHit hit;

            if (Physics.Raycast(mouseRay, out hit))
            {
                hoverNode = hit.transform.GetComponent<GridNode>();
                if(hoverNode != null) {
                    mBuildingGhost.transform.position = hoverNode.transform.position;
                }
            }

            if ((hoverNode != null) && hoverNode.CanBuildHere()) {
                rend.material.color = Color.green;
                if (Input.GetKeyDown(KeyCode.Mouse0)) {
                    mBuildingGhost.StartBuild(hoverNode);
                    mBuildings.Add(mBuildingGhost);
                    hoverNode.Build(mBuildingGhost);
                    mBuildingGhost = null;
                }
            }
            else rend.material.color = Color.red;
                
        }
        
        if(missileTargeting && Input.GetKeyDown(KeyCode.Mouse1)) {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(mouseRay, out hit)) {
                UnqueueBuilding();
                GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/MissileBlast"), hit.point, Quaternion.identity);
            }
        }
	}

	public void UnqueueBuilding() {

        missileTargeting = false;

		if (mBuildingGhost != null) {
			DestroyObject (mBuildingGhost.gameObject);
			mBuildingGhost = null;
		}
	}
	public void QueueBarrier() {
		Debug.Log("making barrier");
        UnqueueBuilding();
        GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/Barrier"), new Vector3(0,0,0), Quaternion.identity);
		mBuildingGhost = obj.GetComponent<Building>();
	}
	public void QueuePylon() {
		Debug.Log("making pylon");
        UnqueueBuilding();
        GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/Pylon"), new Vector3(0,0,0), Quaternion.identity);
		mBuildingGhost = obj.GetComponent<Building>();
	}
	public void QueueLaser() {
		Debug.Log("making missile");
        UnqueueBuilding();
        GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/Missile"), new Vector3(0,0,0), Quaternion.identity);
		mBuildingGhost = obj.GetComponent<Building>();
	}
	public void QueueFactory() {
		Debug.Log("making factory");
        UnqueueBuilding();
        GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/Factory"), new Vector3(0,0,0), Quaternion.identity);
		mBuildingGhost = obj.GetComponent<Building>();
	}

    public void MissileLaunch()
    {
        Debug.Log("launching missile");
        UnqueueBuilding();
        missileTargeting = true;
    }

    void OnGUI()
    {
        if (missileTargeting) {
            //Texture2D texture = new Texture2D(1, 1);
            //texture.SetPixel(0, 0, new Color(1f, 0f, 0f, .5f));
            //texture.Apply();
            //GUI.skin.box.normal.background = texture;
            GUI.DrawTexture(new Rect(Input.mousePosition.x - 50, (-Input.mousePosition.y - 50)+Screen.height, 100, 100),
                Resources.Load("Images/MissileCircle") as Texture);
        }
    }
}
