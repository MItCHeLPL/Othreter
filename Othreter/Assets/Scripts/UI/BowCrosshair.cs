using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BowCrosshair : MonoBehaviour
{
	private GameObject crosshair;
	private Bow bow;

	//public GameObject left;
	//public GameObject right;
	public GameObject dot;

	/*private RectTransform rtLeft;
	private RectTransform rtRight;

	private Vector2 oldPosL;
	private Vector2 oldPosR;

	private RawImage leftTxt;
	private RawImage rightTxt;*/
	private RawImage dotTxt;

	private CameraController cameraController;

	//private Animator anim;

	float speed;

	void Start()
    {
		crosshair = ObjectsMenager.instance.bowCrosshair;
		bow = ObjectsMenager.instance.bow.GetComponent<Bow>();
		cameraController = ObjectsMenager.instance.cam.GetComponent<CameraController>();

		//anim = crosshair.GetComponent<Animator>();

		//rtLeft = left.GetComponent<RectTransform>();
		//rtRight = right.GetComponent<RectTransform>();

		//leftTxt = left.GetComponent<RawImage>();
		//rightTxt = right.GetComponent<RawImage>();
		dotTxt = dot.GetComponent<RawImage>();

		//oldPosL = rtLeft.anchoredPosition;
		//oldPosR = rtRight.anchoredPosition;

		crosshair.SetActive(false);

		speed = (bow.shootForce / bow.maxShootForce);
	}

	void OnGUI()
	{
		if(DataHolder.playerState_Aiming == true && bow.gameObject.activeInHierarchy == true)
		{
			crosshair.SetActive(true);

			/*if (bow.arrowInstantiated == true && bow.arrowReleased == false && bow.shootForce < bow.maxShootForce)
			{
				anim.SetFloat("speed", speed);
				anim.SetBool("play",true);
			}
			else if (bow.arrowInstantiated == false)
			{
				anim.SetBool("play", false);

				rtLeft.anchoredPosition = oldPosL;
				rtRight.anchoredPosition = oldPosR;
			}*/

			
			if((bow.maxShootForce - bow.shootForce < 2.0f) && bow.arrowInstantiated == true)
			{
				//leftTxt.color = new Color32(255, 255, 0, 255);
				//rightTxt.color = new Color32(255, 255, 0, 255);
				dotTxt.color = new Color32(255, 255, 0, 255);
			}
			else if (bow.aimHit.collider != null && (bow.aimHit.transform.tag == "Enemy" || bow.aimHit.transform.tag == "Headshot"))
			{
				//leftTxt.color = new Color32(255, 0, 0, 255);
				//rightTxt.color = new Color32(255, 0, 0, 255);
				dotTxt.color = new Color32(255, 0, 0, 255);
			}
			else
			{
				//leftTxt.color = new Color32(255, 255, 255, 255);
				//rightTxt.color = new Color32(255, 255, 255, 255);
				dotTxt.color = new Color32(255, 255, 255, 255);
			}
		}
		else
		{
			//anim.SetBool("play", false);

			//rtLeft.anchoredPosition = oldPosL;
			//rtRight.anchoredPosition = oldPosR;

			crosshair.SetActive(false);
		}
	}
}
