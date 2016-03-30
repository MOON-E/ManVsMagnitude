using UnityEngine;
using System.Collections;

public class MonsterDebugControls : MonoBehaviour {

    //public MonsterMovementHolder monster;
    public MonsterGridMovement monster;
    public CommandBuffer cb;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
        if (Input.GetKeyDown(KeyCode.UpArrow)) cb.Input(0, "God");
		if (Input.GetKeyDown(KeyCode.DownArrow)) cb.Input(1, "God");
		if (Input.GetKeyDown(KeyCode.LeftArrow)) cb.Input(2, "God");
		if (Input.GetKeyDown(KeyCode.RightArrow)) cb.Input(3, "God");
		if (Input.GetKeyDown(KeyCode.Space)) cb.Input(4, "God");
    }
}
