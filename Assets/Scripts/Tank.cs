using UnityEngine;
using System.Collections;

public class Tank : Unit {
    enum FireState{MOVING, CANFIRE, FIRING};
    public float attackChargeTime;
    float mCharge = 0.0f;

    FireState mFireState;
	// Use this for initialization
	void Start () {
        base.Start();
        CommanderUnitControl mCommander = (CommanderUnitControl)GameObject.FindObjectOfType<CommanderUnitControl>();
	}
	
	// Update is called once per frame
	void Update () {
	    base.Update();
        switch(mFireState){
            case(FireState.CANFIRE):
               // wait for attacks to be issued
                break;
            case FireState.FIRING:
                mCharge += Time.deltaTime;
                if (mCharge == attackChargeTime)
                {
                   // FireAttack();
                }
                mCharge = 0.0f;
                break;
            default:
                break;
        }
	}

    void FireAttack(Vector3 targetPos)
    {
        // Fire a projectile towards targetPos
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

    void ToFireMode()
    {
        mFireState = FireState.CANFIRE;
    }

    void ToMoveMode()
    {
        mFireState = FireState.MOVING;
    }

    
}
