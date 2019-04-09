﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
	public TextMeshProUGUI healthUI;
	public RawImage lockIndicator;
	private Camera cam;
	private UIController uiController;
	private Vector3 pos;
	private EnemyStats enemyStats;
	public float hideDistance = 30.0f;

	void Start()
	{
		cam = ObjectsMenager.instance.cam;
		uiController = ObjectsMenager.instance.UIMenager.GetComponent<UIController>();
		enemyStats = GetComponent<EnemyStats>();

		lockIndicator.enabled = false;

		healthUI.enabled = false;
	}

	void Update()
	{
		if (Vector3.Distance(transform.position, cam.transform.position) < hideDistance && (enemyStats.currentHealth < enemyStats.maxHealth || lockIndicator.enabled == true))
		{
			healthUI.enabled = true;
			uiController.ObjectFaceOtherObject(healthUI.gameObject, cam.gameObject);
		}
		else
		{
			healthUI.enabled = false;
		}
	}

	public void HPChange(float current, float max)
	{
		//healthUI.SetText("Health {0}/{1}", current, max);
		healthUI.SetText("Health {0}", current);
	}
}