using UnityEngine;
using System.Collections;

public class Missile : Building {

	private enum state{CHARGING, READY};
	private state mstate = state.READY;
    public float chargeTime;
    public bool ready = false;
	private float currCharge=0;

	void Update() {
		base.Update ();
		if (mstate == state.CHARGING) {
			currCharge += Time.deltaTime;
			if (currCharge >= chargeTime) {
				GetComponent<MeshRenderer>().material.color = Color.green;
				currCharge = 0;
				mstate = state.READY;
			}
		}
	}

    public void launch()
    {
        if (mstate != state.READY) return;

        Camera.main.GetComponent<CommanderBuildingControl>().MissileLaunch();

        Debug.Log("missile click");
		GetComponent<MeshRenderer>().material.color = Color.red;
		mstate = state.CHARGING;
    }
}
