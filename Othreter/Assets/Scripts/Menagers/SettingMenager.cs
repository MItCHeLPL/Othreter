using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMenager : MonoBehaviour
{
	public float timeScale = 1.0f;

	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	void Start()
    {
		Cursor.lockState = CursorLockMode.Confined;
		Time.timeScale = timeScale;
	}
}
