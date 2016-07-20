﻿using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
    Camera cam;
    public float cameraYSpeed = 0.5f;
    public float cameraXSpeed = 0.5f;
    public float zoomSpeed = 100.0f;
    public float minHeight = 15.0f;
    public float maxHeight = 90.0f;
    public float FOV = 0.0f;
	// Use this for initialization
	void Start () {
	    cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        HandleMove();
        //Debug.Log("FOV: " + cam.fieldOfView);
        //FOV = cam.fieldOfView
    }

    void HandleMove()
    {
        if (Input.mousePosition.x < 0)
        {
            cam.transform.Translate(Vector3.left * cameraXSpeed);
        }
        if (Input.mousePosition.y < 0)
        {
            cam.transform.Translate(Vector3.down * cameraYSpeed);
        }
        if (Input.mousePosition.x > Screen.width)
        {
            cam.transform.Translate(Vector3.right * cameraXSpeed);
        }
        if (Input.mousePosition.y > Screen.height)
        {
            cam.transform.Translate(Vector3.up * cameraYSpeed);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float newZoom = cam.transform.position.y;
        //newZoom -= scroll * zoomSpeed;
        if (scroll < 0)
            newZoom = Mathf.Lerp(newZoom, maxHeight, Time.deltaTime * Mathf.Abs((newZoom - maxHeight) / (minHeight - maxHeight)));
        else if (scroll > 0)
        {
            newZoom = (float).8*Mathf.Lerp(newZoom, minHeight, Time.deltaTime * Mathf.Abs((newZoom - minHeight) / (minHeight - maxHeight)));
        }
        Mathf.Clamp(newZoom, minHeight, maxHeight);
        cam.transform.position = new Vector3(cam.transform.position.x, newZoom, cam.transform.position.z);
    }
}
