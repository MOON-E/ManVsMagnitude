using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tank : Unit {
    enum FireState{MOVING, CANFIRE, FIRING};
    public float attackChargeTime;
    float mCharge = 0.0f;
    public float attackRange = 20f;
    public int attackDamage = 5;

    FireState mFireState;

    public GameObject cannon;
    Renderer cannonRend;

	// Use this for initialization
	void Start () {
        base.Start();
        mFireState = FireState.MOVING;
        cannonRend = cannon.GetComponent<Renderer>();
        CommanderUnitControl mCommander = (CommanderUnitControl)GameObject.FindObjectOfType<CommanderUnitControl>();
	}
	
	// Update is called once per frame
	void Update () {
	    base.Update();

        float color = (1 - (mCharge / attackChargeTime));
        if(cannonRend!=null) cannonRend.material.color = new Color(1, color, color); //charging cannon

        if ((mFireState == FireState.CANFIRE)&&(checkMonsterRange())) {
            mCharge += Time.deltaTime;
            if (mCharge >= attackChargeTime) {
                Debug.Log("Fire!");
                FireAttack();
                mCharge = 0.0f;
            }
        }
        else mCharge = 0.0f;
    }

	public override void Select(bool select)
	{
		Debug.Log ("test");
		base.Select (select);
		GameObject deploy;
		deploy = GameObject.Find ("DeployTanks");
		if (deploy != null) {
			
			if (select) {

				deploy.SetActive (true);
				//GameObject a = (GameObject)Instantiate (alert);
				//a.transform.SetParent (panel.transform, false);
			
			} else {
				deploy.SetActive (false);
				//GameObject a = (GameObject)Instantiate (alert);
				//a.transform.SetParent (panel.transform, false);
			
			}
		}
	}


    void FireAttack()
    {
        MonsterGridMovement monster = null;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);       //Find monster
        foreach (Collider c in hitColliders) {
            if (c.gameObject.GetComponent<MonsterGridMovement>()) monster = c.gameObject.GetComponent<MonsterGridMovement>();
        }

        if(monster!=null) monster.TakeDamage(attackDamage);         //Tell monster to take damage

        LineRenderer shot = Instantiate(Resources.Load("Particles/Shot", typeof(LineRenderer)), transform.position, transform.rotation) as LineRenderer;
        shot.SetPosition(0, transform.position);
        shot.SetPosition(1, monster.transform.position);
    }

    public override void MoveTo(Vector3 destination)
    {
        if (mFireState == FireState.MOVING)
        {
            base.MoveTo(destination);
        }

        else
        {
            // feedback that movement isn't allowed
        }
    }

    public void ToFireMode()
    {
		base.MoveTo(gameObject.transform.position);
        mFireState = FireState.CANFIRE;
        Debug.Log("canfire");
    }

    public void ToMoveMode()
    {
        mFireState = FireState.MOVING;
        Debug.Log("canmove");
    }

    bool checkMonsterRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach(Collider c in hitColliders) {
            if (c.gameObject.GetComponent<MonsterGridMovement>()) return true;
        }
        return false;
    }
}
