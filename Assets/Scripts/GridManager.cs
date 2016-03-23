﻿using UnityEngine;
using System.Collections;

public class GridManager : MonoBehaviour {

    public GameObject[,] grid = new GameObject[10,10];

	// Use this for initialization
	void Awake () {
	    foreach (Transform child in transform) {
            int x, y;
            Vector3 holder = child.transform.position;
            holder += new Vector3(22.5f, 0f, 22.5f);
            x = System.Convert.ToInt32(holder.x * .2f);
            y = System.Convert.ToInt32(holder.z * .2f);

            grid[x, y] = child.gameObject;
            child.name = "Node (" + x + ", " + y + ")";
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Vector3 Find(int x, int y)
    {
        return grid[x, y].transform.position;
    }

    public void Smash(int x, int y)
    {
        grid[x, y].SetActive(false);
    }
}
