using UnityEngine;
using System.Collections;

public class GridNode : MonoBehaviour {

    public int x, y;
    GridNode[] adjacent = new GridNode[8];
    GridManager gm;
    public bool startBase;
    //GameObject pBase;

    public Building building = null;   //Building holder

    public int pylonRange;      //For player power radius

    Renderer rend;

    bool hovering; //for hover select

    public bool onFire;

	void Start () {
        gm = GetComponentInParent<GridManager>();
        rend = GetComponent<Renderer>();

        try { adjacent[0] = gm.FindNode(x, y + 1); }
        catch { adjacent[0] = null; }

        try { adjacent[1] = gm.FindNode(x + 1, y + 1); }
        catch { adjacent[1] = null; }

        try { adjacent[2] = gm.FindNode(x + 1, y); }
        catch { adjacent[2] = null; }

        try { adjacent[3] = gm.FindNode(x + 1, y - 1); }
        catch { adjacent[3] = null; }

        try { adjacent[4] = gm.FindNode(x, y - 1); }
        catch { adjacent[4] = null; }

        try { adjacent[5] = gm.FindNode(x - 1, y - 1); }
        catch { adjacent[5] = null; }

        try { adjacent[6] = gm.FindNode(x - 1, y); }
        catch { adjacent[6] = null; }

        try { adjacent[7] = gm.FindNode(x - 1, y + 1); }
        catch { adjacent[7] = null; }

        if (startBase) {
            building = Instantiate(Resources.Load("Prefabs/PlayerBase", typeof(Building)), transform.position, transform.rotation) as Building;
        }
    }

    void Update () {
        int maxRange = 0;
        if (building != null) maxRange = building.pylonRange;

        foreach (GridNode node in adjacent) {
            if ((node != null) && (node.pylonRange > maxRange)) {
                maxRange = node.pylonRange;
            }
        }

        pylonRange = maxRange - 1;

        if (hovering) rend.material.color = Color.blue;
        else if (maxRange <= 0) rend.material.color = Color.white;
        else rend.material.color = Color.red;
    }

    public void Destroy()
    {
        if (building != null) {
            Destroy(building.gameObject);
            pylonRange = 0;
        }
        Instantiate(Resources.Load("Particles/Demolish Particles"), transform.position, Quaternion.Euler(-90, 0, 0));
        gameObject.SetActive(false);
    }

    void OnMouseEnter()
    {
        hovering = true;
    }

    void OnMouseExit()
    {
        hovering = false;

    }

    public bool CanBuildHere()
    {
        if((pylonRange >= 0) && (building == null)) {
            return true;
        }

        return false;
    }

    void OnMouseDown()
    {
        //Instantiate(building, originalPos, Quaternion.identity);
    }
}
