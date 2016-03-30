using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class alertText : MonoBehaviour {
	

	private Text someText;
	private int lifespan = 5;

	// Use this for initialization
	void Start () {
		someText = GetComponent<Text> ();
		Destroy (gameObject, lifespan);
	}
	
	// Update is called once per frame
	void Update () {
	}

}
