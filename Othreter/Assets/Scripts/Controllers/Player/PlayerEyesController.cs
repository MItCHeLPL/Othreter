using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEyesController : MonoBehaviour
{
	CameraController cam;

    void Start()
    {
        cam = ObjectsMenager.instance.cam.GetComponent<CameraController>();
	}

    void Update()
    {
		if(Input.GetKey(KeyCode.LeftAlt))
		{
			transform.LookAt(cam.transform.position);
		}
		else
		{
			transform.LookAt(transform.forward);
		}
    }
}
