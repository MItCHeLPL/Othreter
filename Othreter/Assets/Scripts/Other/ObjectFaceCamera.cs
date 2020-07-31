using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFaceCamera : MonoBehaviour
{
	private GameObject target;
	[SerializeField] private Vector3 rotationOffset;
	void Start()
	{
		target = ObjectsMenager.instance.cam.gameObject;
	}

    void Update()
    {
		transform.LookAt(target.transform.position, -Vector3.up);
		transform.rotation *= new Quaternion(rotationOffset.x, rotationOffset.y, rotationOffset.z, 0);
	}
}
