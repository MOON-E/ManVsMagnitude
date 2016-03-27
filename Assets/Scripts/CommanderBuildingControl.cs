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
            GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/Barrier"), new Vector3(0,0,0), Quaternion.identity);
            mBuildingGhost = obj.GetComponent<Building>();
           
        }
        if (Input.GetKeyDown(KeyCode.P) && mBuildingGhost == null)
        {
            Debug.Log("making pylon");
            GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/Pylon"), new Vector3(0,0,0), Quaternion.identity);
            mBuildingGhost = obj.GetComponent<Building>();
           
        }
        if (Input.GetKeyDown(KeyCode.L) && mBuildingGhost == null)
        {
            Debug.Log("making building");
            GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/Laser"), new Vector3(0,0,0), Quaternion.identity);
            mBuildingGhost = obj.GetComponent<Building>();
           
        }
        if (Input.GetKeyDown(KeyCode.F) && mBuildingGhost == null)
        {
            Debug.Log("making building");
            GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/Factory"), new Vector3(0,0,0), Quaternion.identity);
            mBuildingGhost = obj.GetComponent<Building>();
           
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
                    mBuildingGhost.StartBuild();
                    mBuildings.Add(mBuildingGhost);
                    hoverNode.Build(mBuildingGhost);
                    mBuildingGhost = null;
                }
            }
            else rend.material.color = Color.red;
                
        }
        

	}
}
