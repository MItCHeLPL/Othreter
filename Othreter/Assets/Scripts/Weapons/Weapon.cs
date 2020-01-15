using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	[Header("Weapon settings")]
	CameraBaseController cameraBaseController;

	[SerializeField] private bool AimCameraEnabled = true;
	[SerializeField] private int AimCameraId = 0;
	private int prevCameraId = 0;

	public float activeWeaponSpeedSub = 0;

	public virtual void Start()
	{
		cameraBaseController = ObjectsMenager.instance.cam.GetComponent<CameraBaseController>();
	}

	public virtual void OnEnable()
	{
		DataHolder.activeWeaponSpeedSub = activeWeaponSpeedSub;
	}

	public virtual void Update()
	{
		if (DataHolder.playerState_Controllable)
		{
			if (Input.GetAxis("Fire2") == 1 || Input.GetMouseButtonDown(1) && DataHolder.playerState_Aiming == false && AimCameraEnabled)
			{
				Aim();
			}
			else if (DataHolder.playerState_Aiming == true && ((Input.GetMouseButtonUp(1) || Input.GetAxis("Fire2") < 1) || (Input.GetMouseButton(1) == false && Input.GetAxis("Fire2") != 1)))
			{
				StopAim();
			}
		}
	}

	public virtual void Aim()
	{
		if (!DataHolder.playerState_Aiming)
		{
			prevCameraId = cameraBaseController.ChangeCamera(AimCameraId);
		}
		DataHolder.playerState_Aiming = true;
	}

	public virtual void StopAim()
	{
		if(DataHolder.playerState_Aiming)
		{
			prevCameraId = cameraBaseController.ChangeCamera(prevCameraId);
		}
		DataHolder.playerState_Aiming = false;
	}
}
