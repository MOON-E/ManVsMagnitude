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
    protected bool isMoving = false;
    protected float maxHP = 100.0f;
    protected float mHP;
    protected static float attackPower = 5.0f;

    //Components
    public Rigidbody rigidbody;
    public NavMeshAgent navmeshagent;

	// Use this for initialization
	protected void Start () {
        rigidbody = GetComponent<Rigidbody>();
        navmeshagent = GetComponent<NavMeshAgent>();
        mHP = maxHP;
	}
	
	// Update is called once per frame
	protected void Update () {
        //if (isMoving)
        //{
        //    Vector3 toEnd = destination - transform.position;

        //    //Calculate pathfinding
        //    //NavMeshPath path = new NavMeshPath();
        //    //navmeshagent.CalculatePath(destination, path);

        //    //for(int i=1; i<path.corners.Length; i++)
        //    //{
        //    //    if (i % 2 == 0)
        //    //        Debug.DrawLine(path.corners[i], path.corners[i - 1], Color.blue);
        //    //    else
        //    //        Debug.DrawLine(path.corners[i], path.corners[i - 1], Color.cyan);
        //    //}

        //    //Vector3 toNextWaypoint = path.corners[1] - transform.position;

        //    //Vector3 moveForce = moveSpeed * toNextWaypoint.normalized;
        //    //rigidbody.AddForce(moveForce, ForceMode.VelocityChange);
        //    //if (toEnd.magnitude <= stopDistance)
        //    //    isMoving = false;

        //    navmeshagent.SetDestination(toEnd);
        //}
	}

    virtual public void MoveTo(Vector3 destination)
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
