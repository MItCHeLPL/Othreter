using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableUI : MonoBehaviour
{
	[SerializeField] private GameObject UIWhenFar;
	[SerializeField] private GameObject UIWhenClose;
	[SerializeField] private float hideDistance = 50.0f;

	private Camera cam;

	void Start()
    {
		cam = ObjectsMenager.instance.cam;

		UIWhenFar.SetActive(false);
		UIWhenClose.SetActive(false);
	}

    void Update()
    {
        if((Vector3.Distance(transform.position, cam.transform.position) < hideDistance) && UIWhenClose.activeInHierarchy == false)
		{
			UIWhenFar.SetActive(true);
		}
		else
		{
			UIWhenFar.SetActive(false);
		}
	}

	public void EnterInteractableArea()
	{
		UIWhenClose.SetActive(true);
		UIWhenFar.SetActive(false);
	}

	public void ExitInteractableArea()
	{
		UIWhenFar.SetActive(true);
		UIWhenClose.SetActive(false);
	}

	public void OnInteraction()
	{
		//Aniamte Interaction
	}
}
