using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
	[SerializeField] private GameObject ui;
	private InteractableUI interacableUI;

	[SerializeField] private UnityEvent InteractionStart;
	[SerializeField] private UnityEvent InteractionCancel;
	[SerializeField] private UnityEvent InteractionSuccess;

	private bool InTrigger = false;

	public bool holdEnabled = false;
	[SerializeField] private float holdTime = 1.0f;
	private bool holding = false;
	private IEnumerator holdCoroutine;

	[SerializeField] private bool cooldownEnabled = false;
	[SerializeField] private float cooldownTime = 1.0f;
	private bool cooldowning = false;

	[SerializeField] private bool limitedUse = false;
	[SerializeField] private int usesLeft = 1;

	void Start()
    {
		interacableUI = ui.GetComponent<InteractableUI>();

		holdCoroutine = Hold(holdTime);
	}

	private void Update()
	{
		if(InTrigger)
		{
			if (Input.GetKeyDown(DataHolder.Interaction) && cooldowning == false)
			{
				InteractionStart.Invoke();

				if (holdEnabled)
				{
					StartCoroutine(holdCoroutine);
				}
				else
				{
					CallOnInteraction(); //call on interaction
				}
			}
			if (Input.GetKeyUp(DataHolder.Interaction) && holding)
			{
				CancelInteraction();
			}
		}
	}

	private void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player")
		{
			InTrigger = true;

			interacableUI.EnterInteractableArea();
		}
	}

	private void OnTriggerExit(Collider col)
	{
		if (col.tag == "Player")
		{
			InTrigger = false;

			interacableUI.ExitInteractableArea();

			if (holding)
			{
				CancelInteraction();
			}
		}
	}

	private void CallOnInteraction()
	{
		if (cooldownEnabled)
		{
			StartCoroutine(Cooldown(cooldownTime));
		}

		if(holdEnabled)
		{
			holdCoroutine = Hold(holdTime); //reset coroutine
			interacableUI.StopHolding();
		}

		interacableUI.OnInteraction(); //ui on interaction

		InteractionSuccess.Invoke(); //invoke event function

		if(limitedUse)
		{
			usesLeft--;
		}

		if(usesLeft == 0)
		{
			gameObject.SetActive(false); //disable interactable if used
		}
	}

	private void CancelInteraction() //if user stopped holding before set time
	{
		StopCoroutine(holdCoroutine);
		holdCoroutine = Hold(holdTime);

		interacableUI.StopHolding();

		InteractionCancel.Invoke();

		holding = false;
	}

	private IEnumerator Cooldown(float sec)
	{
		cooldowning = true;
		yield return new WaitForSeconds(sec);
		cooldowning = false;
	}

	private IEnumerator Hold(float sec)
	{
		holding = true;

		interacableUI.StartHolding(holdTime);

		yield return new WaitForSeconds(sec);

		CallOnInteraction();

		holding = false;
	}

	public void TestDebugStart()
	{
		Debug.Log("Started");
	}

	public void TestDebugCancel()
	{
		Debug.Log("Canceled");
	}

	public void TestDebugSuccess()
	{
		Debug.Log("Success");
	}
}
