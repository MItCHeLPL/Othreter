using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandTorch : Weapon
{
	private PlayerController playerController;
	private Camera cam;


	[SerializeField] private GameObject normalMode;
	[SerializeField] private GameObject aimMode;
	[SerializeField] private GameObject aimLight;

	public override void Start()
	{
		base.Start();

		playerController = ObjectsMenager.instance.player.GetComponent<PlayerController>();
		cam = ObjectsMenager.instance.cam;
	}
	public override void OnEnable()
	{
		base.OnEnable();

		if (aimMode != null)
		{
			normalMode.SetActive(true);
			aimMode.SetActive(false);
		}
	}

	public override void Update()
	{
		base.Update();

		if (DataHolder.playerState_Aiming && aimMode != null)
		{
			aimLight.transform.rotation = Quaternion.Lerp(aimLight.transform.rotation, cam.transform.rotation, 7.5f * Time.deltaTime);
		}
	}


	public override void Aim()
	{
		base.Aim();

		if(aimMode != null)
		{
			aimMode.SetActive(true);
			normalMode.SetActive(false);
		}

		//playermodel rotation
		if (playerController.modelRotationEnabled)
		{
			playerController.transform.rotation = Quaternion.Lerp(playerController.transform.rotation, Quaternion.Euler(0, cam.transform.eulerAngles.y, 0), 7.5f * Time.deltaTime);
			playerController.modelRotation = playerController.transform.rotation;
		}
	}

	public override void StopAim()
	{
		base.StopAim();

		if (aimMode != null)
		{
			normalMode.SetActive(true);
			aimMode.SetActive(false);
		}
	}
}