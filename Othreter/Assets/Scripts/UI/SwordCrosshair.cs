using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCrosshair : MonoBehaviour
{
	GameObject crosshair;
	Sword sword;

	void Start()
	{
		crosshair = ObjectsMenager.instance.swordCrosshair;
		sword = ObjectsMenager.instance.sword.GetComponent<Sword>();
	}

	void Update()
	{
		if (sword.gameObject.activeInHierarchy == true)
		{
			crosshair.SetActive(true);
		}
		else
		{
			crosshair.SetActive(false);
		}

		//dynamic crosshair
	}
}
