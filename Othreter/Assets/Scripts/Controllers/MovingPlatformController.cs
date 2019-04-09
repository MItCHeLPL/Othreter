using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{

	Camera cam;

	private void Start()
	{
		cam = ObjectsMenager.instance.cam;
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.CompareTag("Player"))
		{
			col.transform.parent = transform;
			cam.transform.parent = transform;
		}
	}
	void OnTriggerExit(Collider col)
	{
		if (col.CompareTag("Player"))
		{
			col.transform.parent = null;
			cam.transform.parent = null;
		}
	}
}
