using UnityEngine;
using System.Collections;

public class MonsterAI : MonoBehaviour {
    public float min_time = 0.2f; // how often monster acts

    public CommandBuffer buffer;
    private float timer;

	// Use this for initialization
	void Start () {
            timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
            if (Time.time > timer + min_time) { // trigger monster action
                int action = (int)(Random.value * 100) % 4;
                buffer.Input(action);
                timer = Time.time;
            }
	}
}
