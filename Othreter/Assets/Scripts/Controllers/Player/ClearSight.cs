using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineCollider))]
public class ClearSight : MonoBehaviour
{
	private CinemachineCollider col;
	private CinemachineVirtualCameraBase _middleRig;

	private GameObject player;
	private GameObject cam;

	[SerializeField] private float distance = 2.5f;

	[SerializeField] private GameObject[] objects;

	[SerializeField] private Material transparentMaterial;

	private bool cameraWasDisplaced;

	private void Start()
	{
		player = ObjectsMenager.instance.player;
		cam = ObjectsMenager.instance.cam.gameObject;

		col = this.GetComponent<CinemachineCollider>(); //get collider

		if (col != null)
		{
			var freeLook = col.VirtualCamera as CinemachineFreeLook; //get VCAM
			if (freeLook != null)
				_middleRig = freeLook.GetRig(1); //Get rig required to detect collision
			else
				_middleRig = col.VirtualCamera;
		}
	}

	private void Update()
	{
		if (_middleRig != null)
		{
			cameraWasDisplaced = col.CameraWasDisplaced(_middleRig);
		}
		if (cameraWasDisplaced && (cam.transform.position - player.transform.position).magnitude < distance) //if camera is displaced and is colse enogh to player change materials
		{
			ChangeMaterial();
		}
	}

	private void ChangeMaterial()
	{
		foreach (GameObject obj in objects)
		{
			Renderer objectRenderer = obj.GetComponent<Renderer>();

			if (objectRenderer != null)
			{
				AutoTransparent AT = objectRenderer.GetComponent<AutoTransparent>();
				if (AT == null) // if no script is attached, attach one
				{
					AT = objectRenderer.gameObject.AddComponent<AutoTransparent>();
					AT.TransparentMaterial = transparentMaterial;
				}
				AT.BeTransparent(); // get called every frame to reset the falloff
			}
		}
	}
}