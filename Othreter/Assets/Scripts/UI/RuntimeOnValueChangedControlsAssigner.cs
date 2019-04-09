using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuntimeOnValueChangedControlsAssigner : MonoBehaviour
{
	private Slider slider;
	private InputMenager inputMenager;

	public string axis;

	private void Start()
	{
		slider = GetComponent<Slider>();
		inputMenager = GameObject.FindGameObjectWithTag("GameSettings").GetComponent<InputMenager>();

		slider.onValueChanged.AddListener(delegate {
			inputMenager.GetSlider(slider);
			inputMenager.ChangeSens(axis);
		});

		if(axis == "x")
		{
			slider.value = inputMenager.mouseSensitivityX;
		}
		else if (axis == "y")
		{
			slider.value = inputMenager.mouseSensitivityY;
		}
	}
}