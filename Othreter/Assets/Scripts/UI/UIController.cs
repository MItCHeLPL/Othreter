using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	TextMeshProUGUI healthUI;
	TextMeshProUGUI ammoUI;

	IEnumerator healthFade = null;
	IEnumerator ammoFade = null;

	void Start()
    {
		healthUI = ObjectsMenager.instance.healthUI.GetComponent<TextMeshProUGUI>();
		ammoUI = ObjectsMenager.instance.ammoUI.GetComponent<TextMeshProUGUI>();

		healthUI.enabled = false;
		healthUI.faceColor = new Color32((byte)healthUI.faceColor.r, (byte)healthUI.faceColor.g, (byte)healthUI.faceColor.b, 0);
		healthUI.outlineColor = new Color32((byte)healthUI.outlineColor.r, (byte)healthUI.outlineColor.g, (byte)healthUI.outlineColor.b, 0);
		healthFade = FadeUI(healthUI, 3.0f, 5.0f);

		ammoUI.enabled = false;
		ammoUI.faceColor = new Color32((byte)ammoUI.faceColor.r, (byte)ammoUI.faceColor.g, (byte)ammoUI.faceColor.b, 0);
		ammoUI.outlineColor = new Color32((byte)ammoUI.outlineColor.r, (byte)ammoUI.outlineColor.g, (byte)ammoUI.outlineColor.b, 0);
		ammoFade = FadeUI(ammoUI, 3.0f, 5.0f);
	}

	public void HPChange(int currentArmor, int maxArmor, int currentHp, int maxHp)
	{
		//healthUI.SetText("Health\n{0}/{1}", current, max);
		healthUI.SetText("Armor {0}\nHealth {1}", currentArmor, currentHp);
		StopCoroutine(healthFade);
		healthFade = FadeUI(healthUI, 3.0f, 5.0f);
		StartCoroutine(healthFade);
	}

	public void AmmoChange(int current, int max)
	{
		//ammoUI.SetText("Arrows\n{0}/{1}", current, max);
		ammoUI.SetText("Arrows {0}", current);
		StopCoroutine(ammoFade);
		ammoFade = FadeUI(ammoUI, 3.0f, 5.0f);
		StartCoroutine(ammoFade);
	}

	public void ObjectFaceOtherObject(GameObject obj, GameObject target)
	{
		obj.transform.rotation = Quaternion.LookRotation(new Vector3(target.transform.position.x, 0.0f, target.transform.position.z) - new Vector3(obj.transform.position.x, 0.0f, obj.transform.position.z)) * new Quaternion(0.0f, 180.0f, 0.0f, 0);
	}

	#region Fade Coroutines

	public IEnumerator FadeUI(TextMeshProUGUI ui, float speed, float waitTime)
	{
		if (ui.enabled == false)
		{
			ui.enabled = true;
		}

		while (ui.faceColor.a < 200)
		{
			ui.faceColor = Color32.Lerp(ui.faceColor, new Color32((byte)ui.faceColor.r, (byte)ui.faceColor.g, (byte)ui.faceColor.b, 255), speed * Time.deltaTime);
			ui.outlineColor = Color32.Lerp(ui.outlineColor, new Color32((byte)ui.outlineColor.r, (byte)ui.outlineColor.g, (byte)ui.outlineColor.b, 255), speed * Time.deltaTime);

			yield return new WaitForEndOfFrame();
		}

		ui.faceColor = new Color32((byte)ui.faceColor.r, (byte)ui.faceColor.g, (byte)ui.faceColor.b, 255);
		ui.outlineColor = new Color32((byte)ui.outlineColor.r, (byte)ui.outlineColor.g, (byte)ui.outlineColor.b, 255);

		yield return new WaitForSeconds(waitTime);

		while (ui.faceColor.a != 0)
		{
			ui.faceColor = Color32.Lerp(ui.faceColor, new Color32((byte)ui.faceColor.r, (byte)ui.faceColor.g, (byte)ui.faceColor.b, 0), speed * Time.deltaTime);
			ui.outlineColor = Color32.Lerp(ui.outlineColor, new Color32((byte)ui.outlineColor.r, (byte)ui.outlineColor.g, (byte)ui.outlineColor.b, 0), speed * Time.deltaTime);

			yield return new WaitForEndOfFrame();
		}

		ui.enabled = false;

		ui.faceColor = new Color32((byte)ui.faceColor.r, (byte)ui.faceColor.g, (byte)ui.faceColor.b, 0);
		ui.outlineColor = new Color32((byte)ui.outlineColor.r, (byte)ui.outlineColor.g, (byte)ui.outlineColor.b, 0);
	}

	#endregion
}
