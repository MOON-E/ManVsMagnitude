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

    Building mBuildingGhost = null; // reference to "ghost" building you plan to build
    List<Building> mBuildings = new List<Building>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse1) && mBuildingGhost != null)
        {
            DestroyObject(mBuildingGhost.gameObject);
            mBuildingGhost = null;
        }
        if (Input.GetKeyDown(KeyCode.B) && mBuildingGhost == null)
        {
            Debug.Log("making building");
            GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/TestBuilding"), new Vector3(0,0,0), Quaternion.identity);
            mBuildingGhost = obj.GetComponent<Building>();
           
        }
        if (mBuildingGhost)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit info;
            if (Physics.Raycast(mouseRay, out info))
            {
                mBuildingGhost.transform.position = info.point;
            }
            if (mBuildingGhost.CanBuildHere() && Input.GetKeyDown(KeyCode.Mouse0))
            {
                mBuildingGhost.StartBuild();
                mBuildings.Add(mBuildingGhost);
                mBuildingGhost = null;
            }
        }
        

	}
}
