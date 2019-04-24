using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;

public class DepthOfFieldController : MonoBehaviour
{
	/*private DepthOfField pr;
	public Camera cam;

	private void Start()
	{
		pr = GetComponent<DepthOfField>();
	}

	void Update()
    {
		if (VideoSettingsMenager.video.DoFEnabled)
		{
			RaycastHit hit;
			if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100f))
			{
				pr.focusDistance.value = Vector3.Distance(hit.point, cam.transform.position);
			}
		}
		else
		{
			pr.active = false;
		}
	}*/
}
