using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsMenager : MonoBehaviour
{
	private void Start()
	{
		DontDestroyOnLoad(gameObject);

		#region Game Settings
		#endregion

		#region Video Settings
		if (PlayerPrefs.HasKey("DoFEnabled"))
		{
			if (PlayerPrefs.GetInt("DoFEnabled") == 1)
			{
				DataHolder.DoFEnabled = true;
			}
			else
			{
				DataHolder.DoFEnabled = false;
			}
		}
		else
		{
			if (DataHolder.DoFEnabled == true)
			{
				PlayerPrefs.SetInt("DoFEnabled", 1);
			}
			else
			{
				PlayerPrefs.SetInt("DoFEnabled", 0);
			}
		}
		#endregion

		#region Input Settings
		//Movement
		if (PlayerPrefs.HasKey("jump"))
		{
			DataHolder.Jump = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("jump"));
		}
		else
		{
			PlayerPrefs.SetString("jump", DataHolder.Jump.ToString());
		}

		if (PlayerPrefs.HasKey("crouch"))
		{
			DataHolder.Crouch = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("crouch")); // change to control for default
		}
		else
		{
			PlayerPrefs.SetString("crouch", DataHolder.Crouch.ToString());
		}

		if (PlayerPrefs.HasKey("sprint"))
		{
			DataHolder.Sprint = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("sprint")); // change to shift for default
		}
		else
		{
			PlayerPrefs.SetString("sprint", DataHolder.Sprint.ToString());
		}

		//Camera
		if (PlayerPrefs.HasKey("switchShoulder"))
		{
			DataHolder.SwitchShoulder = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("switchShoulder"));
		}
		else
		{
			PlayerPrefs.SetString("switchShoulder", DataHolder.SwitchShoulder.ToString());
		}

		if (PlayerPrefs.HasKey("zoomIn"))
		{
			DataHolder.ZoomIn = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("zoomIn"));
		}
		else
		{
			PlayerPrefs.SetString("zoomIn", DataHolder.ZoomIn.ToString());
		}

		if (PlayerPrefs.HasKey("zoomOut"))
		{
			DataHolder.ZoomOut = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("zoomOut"));
		}
		else
		{
			PlayerPrefs.SetString("zoomOut", DataHolder.ZoomOut.ToString());
		}

		if (PlayerPrefs.HasKey("mouseSensX"))
		{
			DataHolder.MouseSensitivityX = PlayerPrefs.GetFloat("mouseSensX");
		}
		else
		{
			PlayerPrefs.SetFloat("mouseSensX", DataHolder.MouseSensitivityX);
		}

		if (PlayerPrefs.HasKey("mouseSensY"))
		{
			DataHolder.MouseSensitivityY = PlayerPrefs.GetFloat("mouseSensY");
		}
		else
		{
			PlayerPrefs.SetFloat("mouseSensY", DataHolder.MouseSensitivityY);
		}

		//Combat
		if (PlayerPrefs.HasKey("changeFocus"))
		{
			DataHolder.ChangeFocus = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("changeFocus"));
		}
		else
		{
			PlayerPrefs.SetString("changeFocus", DataHolder.ChangeFocus.ToString());
		}

		//Weapons
		if (PlayerPrefs.HasKey("hideWeapon"))
		{
			DataHolder.HideWeapon = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("hideWeapon"));
		}
		else
		{
			PlayerPrefs.SetString("hideWeapon", DataHolder.HideWeapon.ToString());
		}

		if (PlayerPrefs.HasKey("lastWeapon"))
		{
			DataHolder.LastWeapon = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("lastWeapon"));
		}
		else
		{
			PlayerPrefs.SetString("lastWeapon", DataHolder.LastWeapon.ToString());
		}

		if (PlayerPrefs.HasKey("weaponSlot1"))
		{
			DataHolder.WeaponSlot1 = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("weaponSlot1"));
		}
		else
		{
			PlayerPrefs.SetString("weaponSlot1", DataHolder.WeaponSlot1.ToString());
		}

		if (PlayerPrefs.HasKey("weaponSlot2"))
		{
			DataHolder.WeaponSlot2 = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("weaponSlot2"));
		}
		else
		{
			PlayerPrefs.SetString("weaponSlot2", DataHolder.WeaponSlot2.ToString());
		}

		if (PlayerPrefs.HasKey("weaponSlot3"))
		{
			DataHolder.WeaponSlot3 = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("weaponSlot3"));
		}
		else
		{
			PlayerPrefs.SetString("weaponSlot3", DataHolder.WeaponSlot3.ToString());
		}

		if (PlayerPrefs.HasKey("weaponSlot4"))
		{
			DataHolder.WeaponSlot4 = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("weaponSlot4"));
		}
		else
		{
			PlayerPrefs.SetString("weaponSlot4", DataHolder.WeaponSlot4.ToString());
		}

		if (PlayerPrefs.HasKey("inputDeadzone"))
		{
			DataHolder.inputDeadzone = PlayerPrefs.GetFloat("inputDeadzone");
		}
		else
		{
			PlayerPrefs.SetFloat("inputDeadzone", DataHolder.inputDeadzone);
		}
		#endregion
	}

	private void OnApplicationQuit()
	{
		SaveSettings();
	}

	public void SaveSettings()
	{
		#region Game Settings
		#endregion

		#region Video Settings
		if (DataHolder.DoFEnabled == true)
		{
			PlayerPrefs.SetInt("DoFEnabled", 1);
		}
		else
		{
			PlayerPrefs.SetInt("DoFEnabled", 0);
		}
		#endregion

		#region Input Settings
		//Movement
		PlayerPrefs.SetString("jump", DataHolder.Jump.ToString());
		PlayerPrefs.SetString("crouch", DataHolder.Crouch.ToString());
		PlayerPrefs.SetString("sprint", DataHolder.Sprint.ToString());

		//Camera
		PlayerPrefs.SetString("switchShoulder", DataHolder.SwitchShoulder.ToString());
		PlayerPrefs.SetString("zoomIn", DataHolder.ZoomIn.ToString());
		PlayerPrefs.SetString("zoomOut", DataHolder.ZoomOut.ToString());
		PlayerPrefs.SetFloat("mouseSensX", DataHolder.MouseSensitivityX);
		PlayerPrefs.SetFloat("mouseSensY", DataHolder.MouseSensitivityY);
		PlayerPrefs.SetString("changeFocus", DataHolder.ChangeFocus.ToString());

		//Weapons
		PlayerPrefs.SetString("hideWeapon", DataHolder.HideWeapon.ToString());
		PlayerPrefs.SetString("lastWeapon", DataHolder.LastWeapon.ToString());
		PlayerPrefs.SetString("weaponSlot1", DataHolder.WeaponSlot1.ToString());
		PlayerPrefs.SetString("weaponSlot2", DataHolder.WeaponSlot2.ToString());
		PlayerPrefs.SetString("weaponSlot3", DataHolder.WeaponSlot3.ToString());
		PlayerPrefs.SetString("weaponSlot4", DataHolder.WeaponSlot4.ToString());

		PlayerPrefs.SetFloat("inputDeadzone", DataHolder.inputDeadzone);
		#endregion

		PlayerPrefs.Save();
	}
}