using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
	private int avgFrameRate;
	public TextMeshProUGUI fpsUI;

	public void Update()
	{
		float current = 0;
		current = (int)(1f / Time.unscaledDeltaTime);
		avgFrameRate = (int)current;
		fpsUI.SetText("{0} FPS", avgFrameRate);
	}
}
