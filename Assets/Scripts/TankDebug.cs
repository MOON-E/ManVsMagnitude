using UnityEngine;
using System.Collections;

public class TankDebug : MonoBehaviour {

    public Tank tank;

	// Use this for initialization
	void Start () {
	
	}

    void Update()
    {
        if (tank.isSelected) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) tank.ToFireMode();
            if (Input.GetKeyDown(KeyCode.Alpha2)) tank.ToMoveMode();
        }
    }
}
