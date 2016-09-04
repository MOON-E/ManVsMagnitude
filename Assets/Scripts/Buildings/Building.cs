using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {
    protected enum BuildState{PREBUILD, BUILDING, PAUSED, COMPLETED, DAMAGED};

    protected BuildState mBuildState = BuildState.PREBUILD;
    BoxCollider mCollider;
    public float mBuildTime = 10;
    float timeUntilBuilt;
    [HideInInspector]
    protected int collCounter = 0;

    GridNode location = null;

    Renderer rend;              //to change color

	// Use this for initialization
	public void Start () {
        mCollider = GetComponent<BoxCollider>();
        gameObject.layer = 2; // Ignore raycasts while in Pre-build state
        timeUntilBuilt = mBuildTime;

        rend = gameObject.GetComponent<Renderer>();
       
        /*Color color = GetComponent<MeshRenderer>().material.color;
        color.a = 0.5f;
        GetComponent<MeshRenderer>().material.color = color;*/
              
	}
	
	protected virtual void Update () {
        switch(mBuildState){
            case (BuildState.PREBUILD):
                // While in this state, player is deciding whether or not to build, player will manually change state to BUILDING when needed
                break;
			case (BuildState.BUILDING):

				float c = (timeUntilBuilt / mBuildTime);
				
                rend.material.color = Color.Lerp(Color.yellow, Color.red, c);

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
        //rend.material.color = Color.yellow;

        location = loc;
    }

    void Build(float deltaTime) //enables collisions 
    {
        //Mathf.Lerp(0.0f, 1.0f, .1f); 
        timeUntilBuilt -= deltaTime;
        if (timeUntilBuilt <= float.Epsilon)
        {
			mBuildState = BuildState.COMPLETED;
			rend.material.color = Color.green;
            
            /*Color color = GetComponent<MeshRenderer>().material.color;
            color.a = 1f;
            GetComponent<MeshRenderer>().material.color = color;*/

            location.Activate();
        }
    }

    protected void OnTriggerEnter(Collider coll){
        Debug.Log("Collision enter");
        collCounter++;
    }
    protected void OnTriggerExit(Collider other)
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

	public bool Completed() {
		return (mBuildState == BuildState.COMPLETED);
	}
}
