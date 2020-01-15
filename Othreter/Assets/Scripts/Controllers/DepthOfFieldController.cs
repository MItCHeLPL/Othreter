using UnityEngine;
using UnityEngine.Rendering;
//using UnityEngine.Rendering.HighDefinition;

public class DepthOfFieldController : MonoBehaviour
{
	/*DepthOfField depthOfField;
	private Camera cam;

	private void Start()
	{
		Volume volume = GetComponent<Volume>();
		DepthOfField tempDof;

		if (volume.profile.TryGet<DepthOfField>(out tempDof))
		{
			depthOfField = tempDof;
		}
	}

	private void Update()
    {
		if(DataHolder.DoFEnabled)
		{
			RaycastHit hit;
			if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100.0f))
			{
				depthOfField.focusDistance.value = Vector3.Distance(hit.point, cam.transform.position);
			}
			else
			{
				return;
			}
		}
		else
		{
			return;
		}
	}*/
}