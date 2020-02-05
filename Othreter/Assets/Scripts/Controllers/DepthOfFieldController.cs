using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class DepthOfFieldController : MonoBehaviour
{
	DepthOfField depthOfField;
	private Camera cam;

	private void Start()
	{
		Volume volume = GetComponent<Volume>();
		DepthOfField tempDof;

		cam = ObjectsMenager.instance.cam;

		if (volume.profile.TryGet<DepthOfField>(out tempDof))
		{
			depthOfField = tempDof;
		}
	}

	private void Update()
    {
		Debug.Log(depthOfField.focusDistance.value);
		if(DataHolder.DoFEnabled)
		{
			RaycastHit hit;
			if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100.0f))
			{

				depthOfField.focusDistance.value = Vector3.Distance(hit.point, cam.transform.position);
				Debug.Log("1");
			}
			else
			{
				Debug.Log("2");
				return;
			}
		}
		else
		{
			Debug.Log("3");
			return;
		}
	}
}