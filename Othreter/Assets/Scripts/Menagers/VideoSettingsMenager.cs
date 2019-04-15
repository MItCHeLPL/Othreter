using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoSettingsMenager : MonoBehaviour
{
	public static VideoSettingsMenager video; //singleton

	public bool DoFEnabled = true;

	void Awake()
	{
		video = this; //singleton
	}
}
