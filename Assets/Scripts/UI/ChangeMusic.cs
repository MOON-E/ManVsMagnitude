using UnityEngine;
using System.Collections;

public class ChangeMusic : MonoBehaviour {
    AudioSource source;
    void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    
}
