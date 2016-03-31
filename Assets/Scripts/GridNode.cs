﻿using UnityEngine;
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

    public bool onFire;     //for panic
    GameObject fireParticle;

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
        else rend.material.color = new Color(.2f, .2f, 1f);
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

    public bool Destroy()
    {
		
        Instantiate(buildingDestroyed);
		bool r = DestroyBuilding ();
        Instantiate(Resources.Load("Particles/Demolish Particles"), transform.position, Quaternion.Euler(-90, 0, 0));
        gameObject.SetActive(false);
		return r;
    }

	public bool DestroyBuilding() { //returnss if the destroyed building was a base
		if (building != null) {
			int range = building.pylonRange;
			for (int xi = -1*range; xi <= range; xi++) {
				for (int yi = -1*range; yi <= range; yi++) {
					try {gm.FindNode(x+xi, y+yi).buildStatus -= 1;}
					catch {}
				}
			}
			Destroy(building.gameObject);
			if (startBase) return true;
		}
		return false;
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
        if (onFire) return false;
        if((buildStatus > 0) && (building == null)) {
            return true;
        }

        return false;
    }

    public void Ignite()
    {
        onFire = true;
        fireParticle = Instantiate(Resources.Load("Particles/FirePrefab"), transform.position, transform.rotation) as GameObject;
        Instantiate(Resources.Load("Audio/PanicingCivilianSound"), transform.position, Quaternion.identity);
        StartCoroutine(FireSpread());
    }

    public void Extinguish()
    {
        Destroy(fireParticle);
        onFire = false;
    }

    void OnMouseDown()
    {
        Extinguish();

        if (building is Missile) {
			Missile m = building as Missile;
			m.launch ();
		} 
		if (building is Factory) {
			((Factory)building).StartBuilding();
		}
    }

    IEnumerator FireSpread()
    {
        yield return new WaitForSeconds(10f);
        if (onFire) gm.Panic();
    }
}
