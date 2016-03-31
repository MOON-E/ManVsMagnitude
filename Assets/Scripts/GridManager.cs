using UnityEngine;
using System.Collections;

public class GridManager : MonoBehaviour {

	private enum state{NORMAL, MONSTERWON};
	private state currState = state.NORMAL;
    public int gridWidth, gridHeight;
    GridNode[,] grid = new GridNode[10,10];
	public int numBases = 2;

	public GameObject UICanvas;
	public GameObject MonsterWinCanvas;
	public GameObject monsterWinSound;

	void Update() {
		if (Input.GetKeyDown(KeyCode.R) && currState == state.MONSTERWON)
			Application.LoadLevel (0);
	}

	void Awake () {
	    foreach (Transform child in transform) {            //Get all the grid nodes and store them in array
            int x, y;
            Vector3 holder = child.transform.position;
            holder += new Vector3(22.5f, 0f, 22.5f);
            x = System.Convert.ToInt32(holder.x * .2f);
            y = System.Convert.ToInt32(holder.z * .2f);
            grid[x, y] = child.gameObject.GetComponent<GridNode>();

            child.name = "Node (" + x + ", " + y + ")";     //Label the node and give it coordinates
            child.GetComponent<GridNode>().x = x;
            child.GetComponent<GridNode>().y = y;
        }
	}

    public Vector3 Find(int x, int y)                       //Returns location of node (x, y)
    {
        if (!GridBounds(x, y)) throw new System.Exception("Node location out of bounds");
        return grid[x, y].transform.position;
    }

    public GridNode FindNode(int x, int y)
    {
        if (!GridBounds(x, y)) throw new System.Exception("Node coordinates out of bounds");
        return grid[x, y];
    }

    public bool Smash(int x, int y)                        
    {
        if (!GridBounds(x, y)) return false;
		if (grid [x, y].HasBarrier()) {
			if (grid [x, y].DestroyBuilding ()) numBases--;
			return false;
		} else {
			if (grid [x, y].Destroy ()) numBases--;
		}
		if (numBases == 0)
			MonsterWins ();
		return true;
    }

    bool GridBounds(int x, int y)                           //returns false if node (x, y) is beyond grid limits
    {
        if (x < 0) return false;
        if (y < 0) return false;
        if (x >= gridWidth) return false;
        if (y >= gridHeight) return false;

        return true;
    }

	void MonsterWins() {
		Instantiate (monsterWinSound);
		UICanvas.SetActive (false);
		MonsterWinCanvas.SetActive (true);
		currState = state.MONSTERWON;
	}
}
