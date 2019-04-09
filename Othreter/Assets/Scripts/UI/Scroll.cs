using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour
{
	private Scrollbar scrollbar;
	public float amount = 0.2f;

    void Start()
    {
		scrollbar = GetComponent<Scrollbar>();
	}

    void Update()
    {
		if (Input.GetAxis("Mouse ScrollWheel") > 0f)
		{
			scrollbar.value += amount;
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			scrollbar.value -= amount;
		}
	}
}
