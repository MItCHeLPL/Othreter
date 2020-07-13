using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataHolder
{
	#region Game Settings

	public static float activeWeaponSpeedSub = 0;

	#endregion

	#region Video Settings

	public static bool DoFEnabled { get; set; } = true;

	#endregion

	#region Input Settings
	//Movement
	public static KeyCode Jump { get; set; } = KeyCode.Space;
	public static KeyCode JumpController { get; set; } = KeyCode.JoystickButton0;
	public static KeyCode Crouch { get; set; } = KeyCode.LeftControl;
	public static KeyCode CrouchController { get; set; } = KeyCode.JoystickButton1;
	public static KeyCode Sprint { get; set; } = KeyCode.LeftShift;
	public static KeyCode SprintController { get; set; } = KeyCode.JoystickButton8;

	//Camera
	public static KeyCode SwitchShoulder { get; set; } = KeyCode.C;
	public static KeyCode SwitchShoulderController { get; set; } = KeyCode.JoystickButton3;
	public static KeyCode PauseController { get; set; } = KeyCode.JoystickButton7;
	public static KeyCode BackController { get; set; } = KeyCode.JoystickButton1;
	public static KeyCode SelectController { get; set; } = KeyCode.JoystickButton0;
	public static KeyCode ZoomIn { get; set; } = KeyCode.PageUp;
	public static KeyCode ZoomOut { get; set; } = KeyCode.PageDown;
	public static float MouseSensitivityX { get; set; } = 1.0f;
	public static float MouseSensitivityY { get; set; } = 1.0f;

	//Combat
	public static KeyCode ChangeFocus { get; set; } = KeyCode.Z;
	public static KeyCode ChangeFocusController { get; set; } = KeyCode.JoystickButton9;

	//Weapons
	public static KeyCode HideWeapon { get; set; } = KeyCode.R;
	public static KeyCode HideWeaponController { get; set; } = KeyCode.JoystickButton2;
	public static KeyCode LastWeapon { get; set; } = KeyCode.Q;
	public static KeyCode WeaponSlotUpController { get; set; } = KeyCode.JoystickButton5;
	public static KeyCode WeaponSlotDownController { get; set; } = KeyCode.JoystickButton4;
	public static KeyCode WeaponSlot1 { get; set; } = KeyCode.Alpha1;
	public static KeyCode WeaponSlot2 { get; set; } = KeyCode.Alpha2;
	public static KeyCode WeaponSlot3 { get; set; } = KeyCode.Alpha3;
	public static KeyCode WeaponSlot4 { get; set; } = KeyCode.Alpha4;

	public static float inputDeadzone { get; set; } = 0.01f;
	#endregion

	#region Animator Layers

	public static int BowEquippedLayerId { get; set; } = 1;
	public static int BowAimLayerId { get; set; } = 2;
	public static int BowArrowLayerId { get; set; } = 3;

	#endregion

	#region Player States

	public static bool playerState_Idle { get; set; } = true;
	public static bool playerState_Controllable { get; set; } = true;
	public static bool playerState_Jump { get; set; } = false;
	public static bool playerState_JumpFalling { get; set; } = false;
	public static bool playerState_Falling { get; set; } = false;
	public static bool playerState_Grounded { get; set; } = true;
	public static bool playerState_Sprint { get; set; } = false;
	public static bool playerState_Crouch { get; set; } = false;
	public static bool playerState_Aiming { get; set; } = false;
	public static bool playerState_Fallen { get; set; } = false;
	public static bool playerState_Dead { get; set; } = false;
	public static bool playerState_GotHurt { get; set; } = false;
	public static bool playerState_Sliding { get; set; } = false;
	public static bool playerState_InMenu { get; set; } = false;

	#endregion

	#region Game States



	#endregion

	#region Methods

	public static IEnumerator SetAnimLayer(Animator anim, int layerId, float value, float speed)
	{
		if (anim != null)
		{
			while (anim.GetLayerWeight(layerId) != value)
			{
				anim.SetLayerWeight(layerId, Mathf.Lerp(anim.GetLayerWeight(layerId), value, speed * Time.deltaTime));
				if (Mathf.Abs(anim.GetLayerWeight(layerId) - value) < 0.025f)
				{
					anim.SetLayerWeight(layerId, value);
				}
				yield return null;
			}
		}
	}

	public static void AllowPlayerToControl(bool value)
	{
		playerState_Controllable = value;
	}

	#endregion
}
