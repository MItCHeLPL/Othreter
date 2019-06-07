using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSliderValueAtStart : MonoBehaviour
{
	private Slider slider;

	public string axis;

	private void Start()
	{
		slider = GetComponent<Slider>();

		if(axis == "x")
		{
			slider.value = DataHolder.MouseSensitivityX;
		}
		else if (axis == "y")
		{
			slider.value = DataHolder.MouseSensitivityY;
		}
	}
}