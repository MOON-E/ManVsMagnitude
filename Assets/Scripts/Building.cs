using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {
    enum BuildState{PREBUILD, BUILDING, PAUSED, COMPLETED, DAMAGED};

    BuildState mBuildState;
    BoxCollider mCollider;
    public float mBuildTime = 10;
    float timeUntilBuilt;
    int collCounter = 0;
    public int pylonRange;

    Building(Vector3 Position)
    {
        transform.position = Position;
        mBuildState = BuildState.PREBUILD;
    }


	// Use this for initialization
	void Start () {
        mCollider = GetComponent<BoxCollider>();
        mBuildState = BuildState.PREBUILD;
        gameObject.layer = 2; // Ignore raycasts while in Pre-build state
        timeUntilBuilt = mBuildTime;
       
        
        Color color = GetComponent<MeshRenderer>().material.color;
        color.a = 0.5f;
        GetComponent<MeshRenderer>().material.color = color;
              
	}
	
	// Update is called once per frame
	void Update () {
        switch(mBuildState){
            case (BuildState.PREBUILD):
                // While in this state, player is deciding whether or not to build, player will manually change state to BUILDING when needed
                break;
            case (BuildState.BUILDING):
                Debug.Log("Building");
                Build(Time.deltaTime);
                break;
            case (BuildState.PAUSED): // TODO
                break;
            case (BuildState.COMPLETED): // TODO
                Debug.Log("Building Completed");
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

    public void StartBuild()
    {
        mBuildState = BuildState.BUILDING;
        BoxCollider bc = GetComponent<BoxCollider>();
        if (bc)
        {
            bc.isTrigger = false; // enable collisions with this building
            gameObject.layer = 0;
        }
    }

    void Build(float deltaTime) //enables collisions 
    {
        //Mathf.Lerp(0.0f, 1.0f, .1f); 
        timeUntilBuilt -= deltaTime;
        if (timeUntilBuilt <= float.Epsilon)
        {
            mBuildState = BuildState.COMPLETED;
            Color color = GetComponent<MeshRenderer>().material.color;
            color.a = 1f;
            GetComponent<MeshRenderer>().material.color = color;
        }
    }

    void OnCollisionEnter(Collision coll){
        Debug.Log("Collision enter");
        collCounter++;
    }
    void OnTriggerExit(Collider other)
    {
        collCounter--;
    }
}
