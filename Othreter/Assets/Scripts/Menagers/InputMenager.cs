using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputMenager : MonoBehaviour
{
	//public KeyCode jump { get; set; } //example when using saving data to file

	[Header("Movement")]
	public KeyCode jump = KeyCode.Space;
	public KeyCode crouch = KeyCode.LeftShift; // change to control for default
	public KeyCode sprint = KeyCode.LeftControl;

	[Header("Camera")]
	public KeyCode switchShoulder = KeyCode.C;
	public KeyCode zoomIn = KeyCode.PageUp;
	public KeyCode zoomOut = KeyCode.PageDown;
	public float mouseSensitivityX = 2.0f;
	public float mouseSensitivityY = 1.0f;

	[Header("Combat")]
	public KeyCode changeFocus = KeyCode.Z;

	[Header("Weapons")]
	public KeyCode hideWeapon = KeyCode.R;
	public KeyCode lastWeapon = KeyCode.Q;
	public KeyCode weaponSlot1 = KeyCode.Alpha1;
	public KeyCode weaponSlot2 = KeyCode.Alpha2;
	public KeyCode weaponSlot3 = KeyCode.Alpha3;
	public KeyCode weaponSlot4 = KeyCode.Alpha4;

	public static InputMenager input; //singleton

	private KeyCode action; //for changing bindings
	private Event e;
	private bool waitingForKey = false;
	private KeyCode newKey;
	private Text buttonText;
	private Slider updateSlider;

	void Awake()
	{
		input = this; //singleton

		//jump = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("jumpKey", "Space")); //example when using saving data to file, do this instead of seting value while defining variable
	}

	private void OnGUI()
	{
		e = Event.current;
		if(waitingForKey && e.isKey)
		{
			newKey = e.keyCode;
			waitingForKey = false;
		}
	}

	public void RenameControllButtons(GameObject bindButtons)
	{
		foreach (Transform text in bindButtons.transform)
		{
			switch (text.GetChild(0).GetChild(0).GetComponent<Text>().text)
			{
				case "jump":
					text.GetChild(0).GetChild(0).GetComponent<Text>().text = jump.ToString();
					break;
				case "crouch":
					text.GetChild(0).GetChild(0).GetComponent<Text>().text = crouch.ToString();
					break;
				case "sprint":
					text.GetChild(0).GetChild(0).GetComponent<Text>().text = sprint.ToString();
					break;
				case "switchShoulder":
					text.GetChild(0).GetChild(0).GetComponent<Text>().text = switchShoulder.ToString();
					break;
				case "changeFocus":
					text.GetChild(0).GetChild(0).GetComponent<Text>().text = changeFocus.ToString();
					break;
				case "hideWeapon":
					text.GetChild(0).GetChild(0).GetComponent<Text>().text = hideWeapon.ToString();
					break;
				case "lastWeapon":
					text.GetChild(0).GetChild(0).GetComponent<Text>().text = lastWeapon.ToString();
					break;
				case "weaponSlot1":
					text.GetChild(0).GetChild(0).GetComponent<Text>().text = weaponSlot1.ToString();
					break;
				case "weaponSlot2":
					text.GetChild(0).GetChild(0).GetComponent<Text>().text = weaponSlot2.ToString();
					break;
				case "weaponSlot3":
					text.GetChild(0).GetChild(0).GetComponent<Text>().text = weaponSlot3.ToString();
					break;
				case "weaponSlot4":
					text.GetChild(0).GetChild(0).GetComponent<Text>().text = weaponSlot4.ToString();
					break;
				case "zoomIn":
					text.GetChild(0).GetChild(0).GetComponent<Text>().text = zoomIn.ToString();
					break;
				case "zoomOut":
					text.GetChild(0).GetChild(0).GetComponent<Text>().text = zoomOut.ToString();
					break;
				default:
					break;
			}
		}
	}

	public void StartChange(string action)
	{
		waitingForKey = true;
		StartCoroutine(WaitForKey(action));
	}

	public void SendText(Text text)
	{
		buttonText = text;
	}

	private IEnumerator WaitForKey(string action)
	{
		bool wait = true;
		while (wait == true)
		{
			if (Input.anyKeyDown)
			{
				switch (action)
				{
					case "jump":
						jump = newKey;
						buttonText.text = newKey.ToString();
						break;
					case "crouch":
						crouch = newKey;
						buttonText.text = newKey.ToString();
						break;
					case "sprint":
						sprint = newKey;
						buttonText.text = newKey.ToString();
						break;
					case "switchShoulder":
						switchShoulder = newKey;
						buttonText.text = newKey.ToString();
						break;
					case "changeFocus":
						changeFocus = newKey;
						buttonText.text = newKey.ToString();
						break;
					case "hideWeapon":
						hideWeapon = newKey;
						buttonText.text = newKey.ToString();
						break;
					case "lastWeapon":
						lastWeapon = newKey;
						buttonText.text = newKey.ToString();
						break;
					case "weaponSlot1":
						weaponSlot1 = newKey;
						buttonText.text = newKey.ToString();
						break;
					case "weaponSlot2":
						weaponSlot2 = newKey;
						buttonText.text = newKey.ToString();
						break;
					case "weaponSlot3":
						weaponSlot3 = newKey;
						buttonText.text = newKey.ToString();
						break;
					case "weaponSlot4":
						weaponSlot4 = newKey;
						buttonText.text = newKey.ToString();
						break;
					case "zoomIn":
						zoomIn = newKey;
						buttonText.text = newKey.ToString();
						break;
					case "zoomOut":
						zoomOut = newKey;
						buttonText.text = newKey.ToString();
						break;
					default:
						break;
				}
				wait = false;
			}
			yield return null;
		}
	}

	public void GetSlider(Slider slider)
	{
		updateSlider = slider;
	}

	public void ChangeSens(string axis)
	{
		if(axis == "x")
		{
			mouseSensitivityX = updateSlider.value;
		}
		else if (axis == "y")
		{
			mouseSensitivityY = updateSlider.value;
		}
	}
}