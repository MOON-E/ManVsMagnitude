using UnityEngine;
using System.Collections;

public class Missile : Building {

    public float chargeTime;
    public bool ready = false;
	private float currCharge=0;

    void Start()
    {
        base.Start();
        ready = true;
    }

	void Update() {
		base.Update ();
		if (mBuildState == BuildState.COMPLETED) {
			isAFactory = true;
			ready = true;
		}
		if (!ready) {
			currCharge += Time.deltaTime;
			if (currCharge >= chargeTime) {
				GetComponent<MeshRenderer>().material.color = Color.green;
				ready = true;
				currCharge = 0;
			}
		}
	}

    public void launch()
    {
        if (!ready) return;

        Camera.main.GetComponent<CommanderBuildingControl>().MissileLaunch();

        Debug.Log("missile click");
		GetComponent<MeshRenderer>().material.color = Color.red;
		ready = false;
    }
}
