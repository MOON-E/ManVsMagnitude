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

    public bool isSelected = false;

    public GameObject moveCursor;
    private GameObject mMoveCursor;

	// Use this for initialization
	protected void Start () {
        rigidbody = GetComponent<Rigidbody>();
        navmeshagent = GetComponent<NavMeshAgent>();
        mHP = maxHP;
	}
	
	// Update is called once per frame
	protected void Update () {
        

	}

    virtual public void MoveTo(Vector3 destination)
    {
        this.destination = destination;
        this.destination.y = transform.position.y; //Disable verticality for now
        isMoving = true;

        if (mMoveCursor) { Object.Destroy(mMoveCursor); }
        mMoveCursor = Instantiate(moveCursor, destination, new Quaternion()) as GameObject;

        navmeshagent.SetDestination(destination);
    }

    virtual public void Select(bool select)
    {
        isSelected = select;
        if (select) GetComponent<Renderer>().material.color = Color.blue;
        else GetComponent<Renderer>().material.color = Color.white;
    }

    public Vector2 ScreenPosition()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    virtual public void HandleHotkeyInput()
    {

    }

    virtual public void TakeDamage(float damage)
    {
        mHP -= damage;
        if (mHP <= 0)
        {
            OnDeath();
        }
    }

    virtual public void OnDeath()
    {
        //TODO:
        //all this when monster collides with a unit
        //Destroy this instance
    }

    //void OnDestroy()
    //{
    //    Camera.main.GetComponent<CommanderUnitControl>().DeathReport(this);

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == mMoveCursor)
        {
            GameObject.Destroy(mMoveCursor);
        }
    }
}
