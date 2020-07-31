using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFaceOtherObject : MonoBehaviour
{
	[SerializeField] private GameObject target = default;
	[SerializeField] private Vector3 rotationOffset;

	void Update()
    {
		transform.LookAt(target.transform.position, -Vector3.up);
		transform.rotation *= new Quaternion(rotationOffset.x, rotationOffset.y, rotationOffset.z, 0);
	}
}
