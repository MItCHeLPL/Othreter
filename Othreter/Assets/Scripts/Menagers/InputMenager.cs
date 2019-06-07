using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputMenager : MonoBehaviour
{
	/*[Header("Movement")]
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

	public static InputMenager input; //singleton*/

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
					text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = KeyName(DataHolder.Jump);
					break;
				case "crouch":
					text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = KeyName(DataHolder.Crouch);
					break;
				case "sprint":
					text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = KeyName(DataHolder.Sprint);
					break;
				case "switchShoulder":
					text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = KeyName(DataHolder.SwitchShoulder);
					break;
				case "changeFocus":
					text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = KeyName(DataHolder.ChangeFocus);
					break;
				case "hideWeapon":
					text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = KeyName(DataHolder.HideWeapon);
					break;
				case "lastWeapon":
					text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = KeyName(DataHolder.LastWeapon);
					break;
				case "weaponSlot1":
					text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = KeyName(DataHolder.WeaponSlot1);
					break;
				case "weaponSlot2":
					text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = KeyName(DataHolder.WeaponSlot2);
					break;
				case "weaponSlot3":
					text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = KeyName(DataHolder.WeaponSlot3);
					break;
				case "weaponSlot4":
					text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = KeyName(DataHolder.WeaponSlot4);
					break;
				case "zoomIn":
					text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = KeyName(DataHolder.ZoomIn);
					break;
				case "zoomOut":
					text.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = KeyName(DataHolder.ZoomOut);
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
							DataHolder.Jump = newKey;
							break;
						case "crouch":
							DataHolder.Crouch = newKey;
							break;
						case "sprint":
							DataHolder.Sprint = newKey;
							break;
						case "switchShoulder":
							DataHolder.SwitchShoulder = newKey;
							break;
						case "changeFocus":
							DataHolder.ChangeFocus = newKey;
							break;
						case "hideWeapon":
							DataHolder.HideWeapon = newKey;
							break;
						case "lastWeapon":
							DataHolder.LastWeapon = newKey;
							break;
						case "weaponSlot1":
							DataHolder.WeaponSlot1 = newKey;
							break;
						case "weaponSlot2":
							DataHolder.WeaponSlot2 = newKey;
							break;
						case "weaponSlot3":
							DataHolder.WeaponSlot3 = newKey;
							break;
						case "weaponSlot4":
							DataHolder.WeaponSlot4 = newKey;
							break;
						case "zoomIn":
							DataHolder.ZoomIn = newKey;
							break;
						case "zoomOut":
							DataHolder.ZoomOut = newKey;
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
			DataHolder.MouseSensitivityX = updateSlider.value;
		}
		else if (axis == "y")
		{
			DataHolder.MouseSensitivityY = updateSlider.value;
		}
	}
}