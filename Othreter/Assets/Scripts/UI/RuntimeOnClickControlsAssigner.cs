using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuntimeOnClickControlsAssigner : MonoBehaviour
{
	private Button button;
	private InputMenager inputMenager;

	public string startChangeString;
	public Text text;

	private void Start()
	{
		button = GetComponent<Button>();
		inputMenager = GameObject.FindGameObjectWithTag("GameSettings").GetComponent<InputMenager>();

		button.onClick.AddListener(delegate{
			inputMenager.StartChange(startChangeString);
			inputMenager.SendText(text);
		});
	}
}