using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTrigger : MonoBehaviour
{
	[SerializeField] private UnityEvent TriggerEnter = null;
	[SerializeField] private UnityEvent TriggerStay = null;
	[SerializeField] private UnityEvent TriggerExit = null;

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			TriggerEnter.Invoke();
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			TriggerStay.Invoke();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			TriggerExit.Invoke();
		}
	}
}
