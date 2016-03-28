using UnityEngine;
using System.Collections;

public class LineSelfDestruct : MonoBehaviour {

    public float lifetime = .5f;

	// Use this for initialization
	void Start () {
        StartCoroutine(Lifetime());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
