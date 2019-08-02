using System.Collections;
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
	[SerializeField]
	private float hideDistance = 1000.0f;
	private RaycastHit hit;
	private CameraController camController;

	void Start()
	{
		cam = ObjectsMenager.instance.cam;
		camController = cam.GetComponent<CameraController>();
		uiController = ObjectsMenager.instance.UIMenager.GetComponent<UIController>();
		enemyStats = GetComponent<EnemyStats>();

		lockIndicator.enabled = false;

		healthUI.enabled = false;
	}

	void Update()
	{
		if (Vector3.Distance(transform.position, cam.transform.position) < hideDistance && ((enemyStats.currentHealth < enemyStats.maxHealth || enemyStats.armor.GetValue() < enemyStats.maxArmor) || lockIndicator.enabled == true || ((Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, hideDistance) && camController.aiming) ? (hit.transform == transform || hit.transform.parent == transform) : false)))
		{
			healthUI.enabled = true;
		}
		else
		{
			healthUI.enabled = false;
		}
	}

	public void HPChange(int currentArmor, int maxArmor, int currentHp, int maxHp)
	{
		//healthUI.SetText("Health {0}/{1}", current, max);
		//healthUI.SetText("Health {0}", current);
		healthUI.SetText("Armor {0}\nHealth {1}", currentArmor, currentHp);
	}
}