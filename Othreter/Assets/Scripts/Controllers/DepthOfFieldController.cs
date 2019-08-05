using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;

public class DepthOfFieldController : MonoBehaviour
{
	private DepthOfField dof;
	private Camera cam;

	private void Start()
	{
		dof = GetComponent<DepthOfField>();
		cam = ObjectsMenager.instance.cam;
	}

	private void Update()
    {
		RaycastHit hit;
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100.0f))
		{
			dof.focusDistance.value = Vector3.Distance(hit.point, cam.transform.position);
		}
		else
		{
			return;
		}
	}
}
