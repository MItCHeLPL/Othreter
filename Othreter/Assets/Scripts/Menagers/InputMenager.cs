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
	[HideInInspector]
	public bool waitingForKey = false;
	[HideInInspector]
	public bool wait = false;
	private KeyCode newKey;
	private TextMeshProUGUI buttonText;
	private Slider updateSlider;
	private string previousText;

	void Awake()
	{
		input = this; //singleton

		//jump = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("jumpKey", "Space")); //example when using saving data to file, do this instead of seting value while defining variable
	}

	private void OnGUI()
	{
		e = Event.current;
		if (waitingForKey && e.isKey)
		{
			newKey = e.keyCode;
			waitingForKey = false;
		}
	}

	public void RenameControllButtons(GameObject bindButtons)
	{
		foreach (Transform text in bindButtons.transform)
		{
			switch (text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text)
			{
				case "jump":
					text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = KeyName(jump);
					break;
				case "crouch":
					text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = KeyName(crouch);
					break;
				case "sprint":
					text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = KeyName(sprint);
					break;
				case "switchShoulder":
					text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = KeyName(switchShoulder);
					break;
				case "changeFocus":
					text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = KeyName(changeFocus);
					break;
				case "hideWeapon":
					text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = KeyName(hideWeapon);
					break;
				case "lastWeapon":
					text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = KeyName(lastWeapon);
					break;
				case "weaponSlot1":
					text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = KeyName(weaponSlot1);
					break;
				case "weaponSlot2":
					text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = KeyName(weaponSlot2);
					break;
				case "weaponSlot3":
					text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = KeyName(weaponSlot3);
					break;
				case "weaponSlot4":
					text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = KeyName(weaponSlot4);
					break;
				case "zoomIn":
					text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = KeyName(zoomIn);
					break;
				case "zoomOut":
					text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = KeyName(zoomOut);
					break;
				default:
					break;
			}
		}
	}

	public void StartChange(string action)
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		waitingForKey = true;
		StartCoroutine(WaitForKey(action));
	}

	public void SendText(TextMeshProUGUI text)
	{
		buttonText = text;
		previousText = buttonText.text;
		buttonText.text = "Press key";
	}

	private IEnumerator WaitForKey(string action)
	{
		wait = true;
		while (wait == true)
		{
			if (Input.anyKeyDown)
			{
				if (newKey == KeyCode.Escape)
				{
					Cursor.visible = true;
					Cursor.lockState = CursorLockMode.Confined;
					buttonText.text = previousText;
					wait = false;
				}
				else
				{
					switch (action)
					{
						case "jump":
							jump = newKey;
							break;
						case "crouch":
							crouch = newKey;
							break;
						case "sprint":
							sprint = newKey;
							break;
						case "switchShoulder":
							switchShoulder = newKey;
							break;
						case "changeFocus":
							changeFocus = newKey;
							break;
						case "hideWeapon":
							hideWeapon = newKey;
							break;
						case "lastWeapon":
							lastWeapon = newKey;
							break;
						case "weaponSlot1":
							weaponSlot1 = newKey;
							break;
						case "weaponSlot2":
							weaponSlot2 = newKey;
							break;
						case "weaponSlot3":
							weaponSlot3 = newKey;
							break;
						case "weaponSlot4":
							weaponSlot4 = newKey;
							break;
						case "zoomIn":
							zoomIn = newKey;
							break;
						case "zoomOut":
							zoomOut = newKey;
							break;
						default:
							break;
					}
					buttonText.text = KeyName(newKey);
					Cursor.visible = true;
					Cursor.lockState = CursorLockMode.Confined;
					wait = false;
				}
			}
			yield return null;
		}
	}

	private string KeyName(KeyCode key)
	{
		switch (key)
		{
			case KeyCode.Space:
				return "Space";
			case KeyCode.LeftControl:
				return "LCtrl";
			case KeyCode.LeftShift:
				return "LShift";
			case KeyCode.LeftAlt:
				return "LAlt";
			case KeyCode.PageUp:
				return "PgUp";
			case KeyCode.PageDown:
				return "PgDown";

			case KeyCode.Alpha0:
				return "0";
			case KeyCode.Alpha1:
				return "1";
			case KeyCode.Alpha2:
				return "2";
			case KeyCode.Alpha3:
				return "3";
			case KeyCode.Alpha4:
				return "4";
			case KeyCode.Alpha5:
				return "5";
			case KeyCode.Alpha6:
				return "6";
			case KeyCode.Alpha7:
				return "7";
			case KeyCode.Alpha8:
				return "8";
			case KeyCode.Alpha9:
				return "9";

			default:
				return key.ToString();
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