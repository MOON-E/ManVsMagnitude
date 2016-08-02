using UnityEngine;
using System.Collections;

public class GridManager : MonoBehaviour {

	private enum state{NORMAL, MONSTERWON};
	private state currState = state.NORMAL;
    public int gridWidth, gridHeight;
    GridNode[,] grid = new GridNode[10,10];
	public int numBases = 2;

    public GameObject BuildingNode;

    public GameObject UICanvas;
	public GameObject MonsterWinCanvas;
	public GameObject monsterWinSound;

    System.Random rand;

	void Update() {
		if (Input.GetKeyDown(KeyCode.R) && currState == state.MONSTERWON)
			Application.LoadLevel (0);
	}

	void Awake () {
        //instantiate all of the building nodes
        for (float x = -45; x <= 0; x += 5f)
        {
            for (float z = -45; z <= 0; z += 5f)
            {
                GameObject temp = Instantiate(BuildingNode, new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z), transform.rotation) as GameObject;
                temp.transform.parent = transform;
                if ((x == -35 && z == -10) || (x == -30 && z == -40))
                {
                    temp.GetComponent<GridNode>().startBase = true;
                }
            }
        }

        rand = new System.Random();

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

    public void FireBreath(int x, int y)
    {
        if (!GridBounds(x, y)) return;
        if (grid[x, y].DestroyBuilding()) numBases--;
        grid[x, y].Ignite();
        if (numBases == 0)
            MonsterWins();
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
        if (currState != state.MONSTERWON)
        {
            Instantiate(monsterWinSound);
        }
		UICanvas.SetActive (false);
		MonsterWinCanvas.SetActive (true);
		currState = state.MONSTERWON;
	}

    public void Panic()
    {
        int x = rand.Next(10);
        int y = rand.Next(10);
        grid[x, y].Ignite();
    }
}
