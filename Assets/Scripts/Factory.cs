using UnityEngine;
using System.Collections;

public class Factory : Building{

	private enum state{READY, BUILDING};
	public Unit tankPrefab;
	private Unit tankHolder;
	private state mstate;
	private bool finishedBuilding = false;

	private float currCharge=0;

	public float unitProductionTime;

	public GameObject buildSound;
	public GameObject startBuildSound;
	
	// Update is called once per frame
	void Update () {
		base.Update();
		if (mBuildState == BuildState.COMPLETED) {
			finishedBuilding = true;
		}
		if (mstate == state.BUILDING) {
			currCharge += Time.deltaTime;
			Debug.Log("Building Tank " + currCharge + " / " + unitProductionTime);
			if (currCharge >= unitProductionTime) {
				tankHolder = Instantiate(tankPrefab, new Vector3((float)(transform.position.x), (float)transform.position.y, transform.position.z-1), transform.rotation) as Unit;
				Camera.main.GetComponent<CommanderUnitControl>().units.Add(tankHolder);
				GetComponent<MeshRenderer>().material.color = Color.green;
				mstate = state.READY;
				currCharge = 0;
				Instantiate(buildSound);
			}
		}
	}

	public void StartBuilding() {
		if (mstate != state.READY || !finishedBuilding) return;

		mstate = state.BUILDING;
		GetComponent<MeshRenderer>().material.color = Color.red;
		Instantiate (startBuildSound);
	}
}
