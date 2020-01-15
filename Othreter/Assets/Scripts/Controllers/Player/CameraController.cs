using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
	#region Variables

	private GameObject pauseMenu;
	private CinemachineFreeLook freeLook;

	[Header("Sensitivity")]
	[SerializeField] private float sensitivityX; //Camera sensitivity in x axis
	[SerializeField] private float sensitivityY; //Camera sensitivity in y axis

	[Header("FOV")]
	[SerializeField] private bool sprintFovEnabled = true;

	[SerializeField] private float sprintFov = 65.0f;
	[SerializeField] private float normalFov = 60.0f;

	[Header("Camera Shake")]
	[SerializeField] private bool camShakeEnabled = true;

	private float waitTimer;
	[SerializeField] private float WaitToShake = 1.0f;
	[SerializeField] private float maxShakeAmplitude = 1.0f;

	private CinemachineBasicMultiChannelPerlin noise0;
	private CinemachineBasicMultiChannelPerlin noise1;
	private CinemachineBasicMultiChannelPerlin noise2;

	#endregion

	private void Start()
	{
		pauseMenu = ObjectsMenager.instance.pauseMenu;
		freeLook = GetComponent<CinemachineFreeLook>();
		CinemachineCore.GetInputAxis = GetAxisCustom;

		sensitivityX = DataHolder.MouseSensitivityX;
		sensitivityY = DataHolder.MouseSensitivityY;

		if(camShakeEnabled)
		{
			noise0 = freeLook.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
			noise1 = freeLook.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
			noise2 = freeLook.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		}
	}

	private void Update()
	{
		if (pauseMenu.activeInHierarchy == true)
		{
			sensitivityX = DataHolder.MouseSensitivityX;
			sensitivityY = DataHolder.MouseSensitivityY;
		}

		if(sprintFovEnabled && DataHolder.playerState_Controllable)
		{
			if (DataHolder.playerState_Sprint)
			{
				ChangeFov(sprintFov, 10.0f);
			}
			else
			{
				ChangeFov(normalFov, 10.0f);
			}
		}
	}

	private void LateUpdate()
	{
		if (camShakeEnabled && DataHolder.playerState_Controllable)
		{
			CamShake();
		}
	}

	public float GetAxisCustom(string axisName) //add sensitivity to camera
	{
		if (axisName == "Mouse X" && DataHolder.playerState_Controllable)
		{
			return Input.GetAxis("Mouse X") * sensitivityX * Time.timeScale;
		}
		else if (axisName == "Mouse Y" && DataHolder.playerState_Controllable)
		{
			return Input.GetAxis("Mouse Y") * sensitivityY * Time.timeScale;
		}
		else if(axisName == "Mouse X" && !DataHolder.playerState_Controllable)
		{
			return Input.GetAxis("Mouse X") * 0;
		}
		else if (axisName == "Mouse Y" && !DataHolder.playerState_Controllable)
		{
			return Input.GetAxis("Mouse Y") * 0;
		}
		return 0;
	}

	private void CamShake() //add perlin noise to camera movement
	{
		if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Fire1") != 0 || Input.GetAxis("Fire2") != 0)
		{
			waitTimer = WaitToShake;
			if (noise0.m_AmplitudeGain > 0)
			{
				noise0.m_AmplitudeGain = 0;
				noise1.m_AmplitudeGain = 0;
				noise2.m_AmplitudeGain = 0;
			}
		}

		if (waitTimer > 0)
		{
			waitTimer -= Time.deltaTime;
		}
		else if (waitTimer <= 0)
		{
			noise0.m_AmplitudeGain = Mathf.Clamp(noise0.m_AmplitudeGain + (Time.deltaTime / 2), 0.0f, maxShakeAmplitude);
			noise1.m_AmplitudeGain = Mathf.Clamp(noise1.m_AmplitudeGain + (Time.deltaTime / 2), 0.0f, maxShakeAmplitude);
			noise2.m_AmplitudeGain = Mathf.Clamp(noise2.m_AmplitudeGain + (Time.deltaTime / 2), 0.0f, maxShakeAmplitude);
		}
	}

	private void ChangeFov(float to, float time) //change camera fov
	{
		freeLook.m_Lens.FieldOfView = Mathf.Lerp(freeLook.m_Lens.FieldOfView, to, time * Time.deltaTime);
	}

	public void CancelSwordAim() //temp
	{
		return;
	}
}