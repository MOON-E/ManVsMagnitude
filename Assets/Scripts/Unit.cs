using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
    //Stat variables (unique properties for each unit)
    public float moveSpeed = 1.0f;

    //Variables to tweak (for tuning)
    public float stopDistance = 0.2f;

    //Variables to set
    public Vector3 destination;

    //State Variables
    private bool isMoving = false;

    //Components
    public Rigidbody rigidbody;
    public NavMeshAgent navmeshagent;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
        navmeshagent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void MoveTo(Vector3 destination)
    {
        this.destination = destination;
        this.destination.y = transform.position.y; //Disable verticality for now
        isMoving = true;

        navmeshagent.SetDestination(destination);
    }

    public void Select(bool select)
    {
        if (select) GetComponent<Renderer>().material.color = Color.blue;
        else GetComponent<Renderer>().material.color = Color.white;
    }
}
