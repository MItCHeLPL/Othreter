using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuntimeOnValueChangedControlsAssigner : MonoBehaviour
{
	private Slider slider;

	public string axis;

	private void Start()
	{
		slider = GetComponent<Slider>();

		slider.onValueChanged.AddListener(delegate {
			InputMenager.input.GetSlider(slider);
			InputMenager.input.ChangeSens(axis);
		});

		if(axis == "x")
		{
			slider.value = InputMenager.input.mouseSensitivityX;
		}
		else if (axis == "y")
		{
			slider.value = InputMenager.input.mouseSensitivityY;
		}
	}
}