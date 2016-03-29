using UnityEngine;
using System.Collections;

public class GridNode : MonoBehaviour {

    public int x, y;
    GridManager gm;
    public bool startBase;
    //GameObject pBase;

    public Building building = null;   //Building holder

    public int buildStatus = 0; // 1+ = can build here

    Renderer rend;

    bool hovering; //for hover select

    public bool onFire;


    //AudioSource variables for sound
    public AudioSource buildingBuild;
    public AudioSource buildingDestroyed;

	void Start () {
        
        gm = GetComponentInParent<GridManager>();
        rend = GetComponent<Renderer>();
        
        if (startBase) {
            building = Instantiate(Resources.Load("Prefabs/PlayerBase", typeof(Building)), transform.position, transform.rotation) as Building;
            int range = building.pylonRange;
            for (int xi = -1*range; xi <= range; xi++) {
                for (int yi = -1*range; yi <= range; yi++) {
                    try {gm.FindNode(x+xi, y+yi).buildStatus += 1;}
                    catch {}
                }
            }
        }
    }

    void Update () {
        //if (hovering) rend.material.color = Color.blue;
        if (buildStatus <= 0) rend.material.color = Color.white;
        else rend.material.color = new Color(.5f, .5f, 1f);
    }

    public void Build(Building new_building)
    {
        Instantiate(buildingBuild);
        buildingBuild.Play();
        print("build");
        building = new_building;
    }

    public void Activate()
    {
        int range = building.pylonRange;
        for (int xi = -1*range; xi <= range; xi++) {
            for (int yi = -1*range; yi <= range; yi++) {
                try {gm.FindNode(x+xi, y+yi).buildStatus += 1;}
                catch {}
            }
        }
    }

    public void Destroy()
    {


        Instantiate(buildingDestroyed);
		DestroyBuilding ();
        Instantiate(Resources.Load("Particles/Demolish Particles"), transform.position, Quaternion.Euler(-90, 0, 0));
        gameObject.SetActive(false);
    }

	public void DestroyBuilding() {
		if (building != null) {
			int range = building.pylonRange;
			for (int xi = -1*range; xi <= range; xi++) {
				for (int yi = -1*range; yi <= range; yi++) {
					try {gm.FindNode(x+xi, y+yi).buildStatus -= 1;}
					catch {}
				}
			}
			Destroy(building.gameObject);
		}
	}

	public bool HasBarrier() {
		if (building != null)
			if (building.isABarrier)
				return true;
		return false;
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
        if((buildStatus > 0) && (building == null)) {
            return true;
        }

        return false;
    }

    void OnMouseDown()
    {
        if(building is Missile) {
            Missile m = building as Missile;
            m.launch();
        }
    }
}
