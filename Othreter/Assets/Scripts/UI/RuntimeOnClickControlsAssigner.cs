using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RuntimeOnClickControlsAssigner : MonoBehaviour
{
	private Button button;
	//private InputMenager inputMenager;

	public string startChangeString;
	public TextMeshProUGUI text;

	private void Start()
	{
		button = GetComponent<Button>();
		//inputMenager = GameObject.FindGameObjectWithTag("GameSettings").GetComponent<InputMenager>();

		button.onClick.AddListener(delegate{
			//inputMenager.StartChange(startChangeString);
			//inputMenager.SendText(text);
			InputMenager.input.StartChange(startChangeString);
			InputMenager.input.SendText(text);
		});
	}
}