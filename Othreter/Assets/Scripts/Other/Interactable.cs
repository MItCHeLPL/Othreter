using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
	[SerializeField] private GameObject ui;
	private InteractableUI interacableUI;
	[SerializeField] private UnityEvent OnInteraction;

	[SerializeField] private float cooldownTime = 0.0f;
	private bool cooldowning = false;
	[SerializeField] private bool limitedUse = false;
	[SerializeField] private int usesLeft = 1;

	void Start()
    {
		interacableUI = ui.GetComponent<InteractableUI>();
	}

	private void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player")
		{
			interacableUI.EnterInteractableArea();
		}
	}

	private void OnTriggerStay(Collider col)
	{
		if(col.tag == "Player")
		{
			if(Input.GetKeyDown(DataHolder.Interaction) && cooldowning == false)
			{
				CallOnInteraction(); //call on interaction
			}
		}
	}

	private void OnTriggerExit(Collider col)
	{
		if (col.tag == "Player")
		{
			interacableUI.ExitInteractableArea();
		}
	}

	private void CallOnInteraction()
	{
		StartCoroutine(Cooldown(cooldownTime));
		interacableUI.OnInteraction();
		OnInteraction.Invoke();
		if(limitedUse)
		{
			usesLeft--;
		}
		if(usesLeft == 0)
		{
			gameObject.SetActive(false);
		}
	}

	private IEnumerator Cooldown(float sec)
	{
		cooldowning = true;
		yield return new WaitForSeconds(sec);
		cooldowning = false;
	}

	public void TestDebug()
	{
		Debug.Log("Interacted");
	}
}
