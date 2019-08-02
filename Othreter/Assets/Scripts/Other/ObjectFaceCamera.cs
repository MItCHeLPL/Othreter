using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFaceCamera : MonoBehaviour
{
	private GameObject target;
	void Start()
	{
		target = ObjectsMenager.instance.cam.gameObject;
	}

    void Update()
    {
		transform.rotation = Quaternion.LookRotation(new Vector3(target.transform.position.x, 0.0f, target.transform.position.z) - new Vector3(transform.position.x, 0.0f, transform.position.z)) * new Quaternion(0.0f, 180.0f, 0.0f, 0);
	}
}
