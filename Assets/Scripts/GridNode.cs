using UnityEngine;
using System.Collections;

public class GridNode : MonoBehaviour {

    public int x, y;
    GridNode[] adjacent = new GridNode[8];
    GridManager gm;
    public bool hasBase;
    GameObject pBase;

    public int pylonRange;      //For player power radius

	void Start () {
        gm = GetComponentInParent<GridManager>();

        try { adjacent[0] = gm.FindNode(x, y + 1); }
        catch { adjacent[0] = null; }

        try { adjacent[1] = gm.FindNode(x, y - 1); }
        catch { adjacent[0] = null; }

        try { adjacent[2] = gm.FindNode(x - 1, y); }
        catch { adjacent[0] = null; }

        try { adjacent[3] = gm.FindNode(x + 1, y); }
        catch { adjacent[0] = null; }

        try { adjacent[4] = gm.FindNode(x + 1, y + 1); }
        catch { adjacent[0] = null; }

        try { adjacent[5] = gm.FindNode(x + 1, y - 1); }
        catch { adjacent[0] = null; }

        try { adjacent[6] = gm.FindNode(x - 1, y - 1); }
        catch { adjacent[0] = null; }

        try { adjacent[7] = gm.FindNode(x - 1, y + 1); }
        catch { adjacent[0] = null; }

        if (hasBase) {
            pBase = Instantiate(Resources.Load("Prefabs/PlayerBase", typeof(GameObject)), transform.position, transform.rotation) as GameObject;
        }
    }
	
	void Update () {
        int maxRange = 0;
        if (hasBase) maxRange = 3;

        foreach (GridNode node in adjacent) {
            if ((node != null) && (node.pylonRange > maxRange)) {
                maxRange = node.pylonRange;
            }
        }

        pylonRange = maxRange - 1;

        if (maxRange == 0) GetComponent<Renderer>().material.color = Color.white;
        else GetComponent<Renderer>().material.color = Color.red;
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
        Destroy(pBase);
        hasBase = false;
        Instantiate(Resources.Load("Particles/Demolish Particles"), transform.position, Quaternion.Euler(-90, 0, 0));
        pylonRange = 0;
    }
}
