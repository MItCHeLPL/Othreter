using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBaseController : MonoBehaviour
{
	[SerializeField] private List<CinemachineFreeLook> cameras = new List<CinemachineFreeLook>();
	private CinemachineBrain brain;

	private int prevCamId = 0;

	private void Start()
	{
		brain = GetComponent<CinemachineBrain>();
	}

	public int ChangeCamera(int camId)
	{
		brain.ActiveVirtualCamera.Priority = 10;
		
		for(int i = 0; i < cameras.Count; i++)
		{
			if(brain.ActiveVirtualCamera.VirtualCameraGameObject == cameras[i].VirtualCameraGameObject)
			{
				prevCamId = i;
				break;
			}
		}

		cameras[camId].GetComponent<CinemachineFreeLook>().Priority = 99;

		return prevCamId; //return previous camera
	}
}
