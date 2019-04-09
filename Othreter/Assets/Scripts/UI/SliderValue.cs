using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
	private TextMeshProUGUI val;
	private Slider slider;

	void Start()
	{
		val = GetComponent<TextMeshProUGUI>();
		slider = transform.parent.GetComponent<Slider>();

		val.SetText("{0:2}", slider.value);
	}

	private void Update()
	{
		val.SetText("{0:2}", slider.value);
	}
}
