﻿using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {
    protected enum BuildState{PREBUILD, BUILDING, PAUSED, COMPLETED, DAMAGED};

    protected BuildState mBuildState;
    BoxCollider mCollider;
    public float mBuildTime = 10;
    float timeUntilBuilt;
    public int collCounter = 0;
    public int pylonRange;
    public bool isABarrier = false;
	public bool isAFactory = false;

    GridNode location = null;

	// Use this for initialization
	public void Start () {
        mCollider = GetComponent<BoxCollider>();
        mBuildState = BuildState.BUILDING;
        gameObject.layer = 2; // Ignore raycasts while in Pre-build state
        timeUntilBuilt = mBuildTime;
       
        /*Color color = GetComponent<MeshRenderer>().material.color;
        color.a = 0.5f;
        GetComponent<MeshRenderer>().material.color = color;*/
              
	}
	
	// Update is called once per frame
	protected void Update () {
        switch(mBuildState){
            case (BuildState.PREBUILD):
                // While in this state, player is deciding whether or not to build, player will manually change state to BUILDING when needed
                break;
            case (BuildState.BUILDING):
                Debug.Log("Building " + (mBuildTime-timeUntilBuilt).ToString() + "/" + (mBuildTime).ToString());
                Build(Time.deltaTime);
                break;
            case (BuildState.PAUSED): // TODO
                break;
            case (BuildState.COMPLETED): // TODO
                //Debug.Log("Building Completed");
                break;
            case (BuildState.DAMAGED): //TODO
                break;
            default:
                break;
        };
            
	}

    public bool CanBuildHere(){
        if (mCollider != null && collCounter == 0)
        {
            return true;
        }
        return false;
    }

    public void StartBuild(GridNode loc)
    {
        mBuildState = BuildState.BUILDING;
        BoxCollider bc = GetComponent<BoxCollider>();
        if (bc)
        {
            bc.isTrigger = false; // enable collisions with this building
            gameObject.layer = 0;
        }
        GetComponent<MeshRenderer>().material.color = Color.yellow;

        location = loc;
    }

    void Build(float deltaTime) //enables collisions 
    {
        //Mathf.Lerp(0.0f, 1.0f, .1f); 
        timeUntilBuilt -= deltaTime;
        if (timeUntilBuilt <= float.Epsilon)
        {
            mBuildState = BuildState.COMPLETED;
            GetComponent<MeshRenderer>().material.color = Color.green;

            /*Color color = GetComponent<MeshRenderer>().material.color;
            color.a = 1f;
            GetComponent<MeshRenderer>().material.color = color;*/

            location.Activate();
        }
    }

    void OnTriggerEnter(Collider coll){
        Debug.Log("Collision enter");
        collCounter++;
    }
    void OnTriggerExit(Collider other)
    {
        collCounter--;
    }
    void OnTriggerStay(Collider other)
    {
        if (collCounter == 0)
        {
            collCounter = 1;
        }
    }
    void OnCollisionEnter(Collision coll)
    {
        collCounter++;
    }
    void OnCollisionExit(Collision coll)
    {
        collCounter--;
    }
    void OnCollisionStay(Collision other)
    {
        if (collCounter == 0)
        {
            collCounter = 1;
        }
    }
   
}
