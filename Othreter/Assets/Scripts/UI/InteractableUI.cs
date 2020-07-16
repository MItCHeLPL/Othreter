using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableUI : MonoBehaviour
{
	[SerializeField] private GameObject UIWhenFar;
	[SerializeField] private GameObject UIWhenClose;
	[SerializeField] private GameObject UIWhenCloseHold;
	[SerializeField] private GameObject HoldSliderGameObject;

	[SerializeField] private float hideDistance = 50.0f;

	private Camera cam;
	private Interactable interactable;

	private Image holdSlider;
	private bool holding = false;
	private float holdMaxValue = 1.0f;

	void Start()
    {
		cam = ObjectsMenager.instance.cam;

		interactable = gameObject.GetComponentInParent<Interactable>();

		if(UIWhenCloseHold != null)
		{
			holdSlider = HoldSliderGameObject.GetComponent<Image>();
		}

		UIWhenFar.SetActive(false);
		UIWhenClose.SetActive(false);
		if (UIWhenCloseHold != null)
		{
			UIWhenCloseHold.SetActive(false);
		}
	}

    void Update()
    {
        if((Vector3.Distance(transform.position, cam.transform.position) < hideDistance) && (UIWhenClose.activeInHierarchy == false && UIWhenCloseHold.activeInHierarchy == false))
		{
			UIWhenFar.SetActive(true);
		}
		else
		{
			UIWhenFar.SetActive(false);
		}

		if(holding)
		{
			holdSlider.fillAmount += (1/holdMaxValue) * Time.deltaTime; //fill slider over time
		}
	}

	public void EnterInteractableArea()
	{
		if (interactable.holdEnabled && UIWhenCloseHold != null)
		{
			UIWhenCloseHold.SetActive(true);
		}
		else
		{
			UIWhenClose.SetActive(true);
		}

		UIWhenFar.SetActive(false);
	}

	public void ExitInteractableArea()
	{
		UIWhenFar.SetActive(true);

		if (interactable.holdEnabled && UIWhenCloseHold != null)
		{
			UIWhenCloseHold.SetActive(false);
		}
		else
		{
			UIWhenClose.SetActive(false);
		}
	}

	public void StartHolding(float sec)
	{
		if (UIWhenCloseHold != null)
		{
			holdSlider.fillAmount = 0;//prepare hold slider

			holdMaxValue = sec;

			holding = true;
		}
	}

	public void StopHolding() 
	{
		if (UIWhenCloseHold != null)
		{
			holdSlider.fillAmount = 0; //reset hold slider
			holding = false;
		}
	}

	public void OnInteraction()
	{
		//Aniamte Interaction
	}
}
