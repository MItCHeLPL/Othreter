using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OverHeadUI : MonoBehaviour
{
	public TextMeshProUGUI healthUI;
	private Camera cam;
	private Vector3 pos;

	void Start()
    {
		cam = ObjectsMenager.instance.cam;
    }

    void Update()
    {
		pos = cam.WorldToScreenPoint(transform.position);
		healthUI.transform.position = pos;
    }

	public void HPChange(float current, float max)
	{
		healthUI.SetText("Health\n{0}/{1}", current, max);
	}
}
