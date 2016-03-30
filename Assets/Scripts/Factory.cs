using UnityEngine;
using System.Collections;

public class Factory : Building{

	private enum state{READY, BUILDING};
	public Unit tankPrefab;
	private Unit tankHolder;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
		if (mBuildState == BuildState.COMPLETED) {
			isAFactory = true;
		}
	}

	public void StartBuilding() {
		//Debug.Log ("memes");
		if (isAFactory == true) {
			tankHolder = Instantiate(tankPrefab, new Vector3((float)(transform.position.x), (float)transform.position.y, transform.position.z-1), transform.rotation) as Unit;
			Camera.main.GetComponent<CommanderUnitControl>().units.Add(tankHolder);
		}
	}
}
