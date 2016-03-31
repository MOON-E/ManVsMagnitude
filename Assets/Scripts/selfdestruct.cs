using UnityEngine;
using System.Collections;

public class selfdestruct : MonoBehaviour {
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if(!gameObject.GetComponent<AudioSource>().isPlaying)
            Destroy(gameObject);
	}
}
