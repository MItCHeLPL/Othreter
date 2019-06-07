using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataHolder
{
	#region Game Settings
	#endregion

	#region Video Settings
	public static bool DoFEnabled { get; set; } = false;
	#endregion

	#region Input Settings
	//Movement
	public static KeyCode Jump { get; set; } = KeyCode.Space;
	public static KeyCode Crouch { get; set; } = KeyCode.LeftControl;
	public static KeyCode Sprint { get; set; } = KeyCode.LeftShift;

	//Camera
	public static KeyCode SwitchShoulder { get; set; } = KeyCode.C;
	public static KeyCode ZoomIn { get; set; } = KeyCode.PageUp;
	public static KeyCode ZoomOut { get; set; } = KeyCode.PageDown;
	public static float MouseSensitivityX { get; set; } = 2.0f;
	public static float MouseSensitivityY { get; set; } = 1.0f;

	//Combat
	public static KeyCode ChangeFocus { get; set; } = KeyCode.Z;

	//Weapons
	public static KeyCode HideWeapon { get; set; } = KeyCode.R;
	public static KeyCode LastWeapon { get; set; } = KeyCode.Q;
	public static KeyCode WeaponSlot1 { get; set; } = KeyCode.Alpha1;
	public static KeyCode WeaponSlot2 { get; set; } = KeyCode.Alpha2;
	public static KeyCode WeaponSlot3 { get; set; } = KeyCode.Alpha3;
	public static KeyCode WeaponSlot4 { get; set; } = KeyCode.Alpha4;
	#endregion
}
