using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySettingMenager : MonoBehaviour
{
	public float timeScale = 1.0f;

	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	void Start()
    {
		Time.timeScale = timeScale;
	}
}
