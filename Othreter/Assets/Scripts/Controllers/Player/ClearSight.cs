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

	[SerializeField] private float distance = 2.5f;

	[SerializeField] private GameObject[] objects;

	[SerializeField] private Material transparentMaterial;

	private void Start()
	{
		player = ObjectsMenager.instance.player;

		col = this.GetComponent<CinemachineCollider>();
		if (col != null)
		{
			var freeLook = col.VirtualCamera as CinemachineFreeLook;
			if (freeLook != null)
				_middleRig = freeLook.GetRig(1);
		}
	}

	private void Update()
	{
		if (_middleRig != null && (col.CameraWasDisplaced(_middleRig) || col.IsTargetObscured(_middleRig)) && distance != 0 && Vector3.Distance(col.transform.position, player.transform.position) < distance)
		{
			ChangeMaterial();
		}
		else if(_middleRig != null && (col.CameraWasDisplaced(_middleRig) || col.IsTargetObscured(_middleRig)) && distance == 0)
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