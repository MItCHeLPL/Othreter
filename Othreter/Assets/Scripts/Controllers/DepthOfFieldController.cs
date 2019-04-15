using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DepthOfFieldController : MonoBehaviour
{
	private PostProcessVolume postProcessVolume;
	void Start()
    {
		postProcessVolume = GetComponent<PostProcessVolume>();
	}

    void Update()
    {
		if (postProcessVolume && VideoSettingsMenager.video.DoFEnabled)
		{
			DepthOfField pr;
			if (postProcessVolume.sharedProfile.TryGetSettings<DepthOfField>(out pr))
			{
				RaycastHit hit;
				if (Physics.Raycast(transform.position, transform.forward, out hit, 100f))
				{
					pr.focusDistance.value = Vector3.Distance(hit.point, transform.position);
				}
				
			}
		}
		else
		{
			DepthOfField pr;
			if (postProcessVolume.sharedProfile.TryGetSettings<DepthOfField>(out pr))
			{
				pr.active = false;
			}
		}
	}
}
