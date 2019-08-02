using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFaceOtherObject : MonoBehaviour
{
	[SerializeField]
	private GameObject obj;

	[SerializeField]
	private GameObject target;

    void Update()
    {
		obj.transform.rotation = Quaternion.LookRotation(new Vector3(target.transform.position.x, 0.0f, target.transform.position.z) - new Vector3(obj.transform.position.x, 0.0f, obj.transform.position.z)) * new Quaternion(0.0f, 180.0f, 0.0f, 0);
	}
}
