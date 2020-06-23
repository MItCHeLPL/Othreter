using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineCollider))]
public class ClearSight : MonoBehaviour
{
	private bool CameraWasDisplaced;
	private CinemachineCollider col;
	private CinemachineVirtualCameraBase _middleRig;

	[SerializeField] private GameObject[] objects;
	[SerializeField] private Material transparentMaterial;

	void Start()
	{
		col = this.GetComponent<CinemachineCollider>();
		if (col != null)
		{
			var freeLook = col.VirtualCamera as CinemachineFreeLook;
			if (freeLook != null)
				_middleRig = freeLook.GetRig(1);
		}
	}

	void Update()
	{
		if (_middleRig != null)
		{
			CameraWasDisplaced = col.CameraWasDisplaced(_middleRig);
		}

		if(CameraWasDisplaced)
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
}