using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RuntimeOnClickControlsAssigner : MonoBehaviour
{
	private Button button;
	public string startChangeString;
	public TextMeshProUGUI text;

	private void Start()
	{
		button = GetComponent<Button>();

		button.onClick.AddListener(delegate{
			InputMenager.input.StartChange(startChangeString);
			InputMenager.input.SendText(text);
		});
	}
}