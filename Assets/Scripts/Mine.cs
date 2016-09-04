using UnityEngine;
using System.Collections;

public class Mine : Building {

    public float baseDamage = 10;
    public float radius = 10.0f; // decides the radius of the explosion
    public float detonationTimer = 1.0f;
    ArrayList objectsInBlastZone;
    public SphereCollider explosion;
    public CapsuleCollider mTrigger;

	// Use this for initialization
	void Start () {
        base.Start();
        mTrigger = GetComponent<CapsuleCollider>();
        explosion = GetComponent<SphereCollider>();
        explosion.radius = radius;
        explosion.enabled = false;
        explosion.isTrigger = true;
        mTrigger.isTrigger = true;
        objectsInBlastZone = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
        if (explosion.enabled)
        {
            detonationTimer -= Time.deltaTime;
        }
        if (detonationTimer <= 0)
        {
            Detonate();
        }
	}

    void OnTriggerEnter(Collider coll)
    {
        if (!base.Completed())
        {
            base.OnTriggerEnter(coll);
            return;
        }
        if (coll.gameObject.GetComponent<MonsterGridMovement>() && !explosion.enabled)
        {
            TriggerMine();
            Debug.Log("Monster hit mine");
        }
        if ((coll.gameObject.GetComponent<MonsterGridMovement>() || coll.gameObject.GetComponent<Unit>()) && explosion.enabled)
        {
            objectsInBlastZone.Add(coll.gameObject);
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (!base.Completed())
        {
            base.OnTriggerExit(coll);
            return;
        }
        if (explosion.enabled && (coll.gameObject.GetComponent<Unit>() || coll.gameObject.GetComponent<MonsterGridMovement>()))
        {
            objectsInBlastZone.Remove(coll.gameObject);
        }
    }

    private void TriggerMine()
    {
        explosion.enabled = true;
        mTrigger.enabled = false;
        Debug.Log("Mine Triggered");
    }

    //Play effect, damages everything in range, and destroys itself
    private void Detonate()
    {
        Debug.Log("Mine Detonated");
        //TODO: Play any explosion effects:

        // deal damage
        foreach (GameObject obj in objectsInBlastZone)
        {
            if (obj.GetComponent<MonsterGridMovement>())
            {
                obj.GetComponent<MonsterGridMovement>().TakeDamage((int)baseDamage); // TODO: maybe get rid of typecast, change monster's TakeDamage to use floats?
            }
            else if (obj.GetComponent<Unit>())
            {
                obj.GetComponent<Unit>().TakeDamage(baseDamage);
            }
        }
        //die
        GameObject.Destroy(gameObject);
    }
}
