using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
	CameraController cameraController;
	GameObject pauseMenu;

	//private GameObject[] weapons;
	[SerializeField] private List<GameObject> weapons = new List<GameObject>();

	private int index = 0;
	private int prevIndex = 1;

	bool weaponsHidden = true;
	bool isSwitching = false;

	void Start()
    {
		cameraController = ObjectsMenager.instance.cam.GetComponent<CameraController>();
		pauseMenu = ObjectsMenager.instance.pauseMenu;

		for (int i = 0; i < weapons.Count; i++)
		{
			weapons[i].SetActive(false);
		}
		weapons[0].SetActive(true);
    }
	
    void Update()
    {
		if (isSwitching == false && cameraController.aiming == false && pauseMenu.activeInHierarchy == false)
		{
			if (Input.GetKeyDown(InputMenager.input.hideWeapon))
			{
				if (index == 0)
				{
					SwitchWeapon(prevIndex);
					weaponsHidden = false;
				}
				else
				{
					SwitchWeapon(0);
					weaponsHidden = true;
				}
			}

			if (Input.GetKeyDown(InputMenager.input.lastWeapon))
			{
				SwitchWeapon(prevIndex);
			}

			/*if (Input.GetAxis("Mouse ScrollWheel") > 0 && weaponsHidden == false)
			{
				if (index == weapons.Count - 1)
				{
					SwitchWeapon(1);
				}
				else
				{
					SwitchWeapon(index + 1);
				}
			}

			if (Input.GetAxis("Mouse ScrollWheel") < 0 && weaponsHidden == false)
			{
				if (index == 1)
				{
					SwitchWeapon(weapons.Count - 1);
				}
				else
				{
					SwitchWeapon(index - 1);
				}
			}*/


			if (Input.GetKeyDown(InputMenager.input.weaponSlot1))
			{
				SwitchWeapon(1);
				weaponsHidden = false;
			}

			if (Input.GetKeyDown(InputMenager.input.weaponSlot2) && weapons.Count > 2)
			{
				SwitchWeapon(2);
				weaponsHidden = false;
			}

			if (Input.GetKeyDown(InputMenager.input.weaponSlot3) && weapons.Count > 3)
			{
				SwitchWeapon(3);
				weaponsHidden = false;
			}

			if (Input.GetKeyDown(InputMenager.input.weaponSlot4) && weapons.Count > 4)
			{
				SwitchWeapon(4);
				weaponsHidden = false;
			}
		}
	}

	private void SwitchWeapon(int newIndex)
	{
		if(weaponsHidden == false)
		{
			//play anim hide weapon
		}

		weapons[index].SetActive(false);
		weapons[newIndex].SetActive(true);
		prevIndex = index;
		index = newIndex;

		StartCoroutine(Cooldown(0.75f));

		cameraController.weaponDistanceChangeDone = false;

		//play anim draw weapon
	}

	private IEnumerator Cooldown(float sec)
	{
		isSwitching = true;
		yield return new WaitForSeconds(sec);
		isSwitching = false;
	}

	public string ActiveWeaponTag()
	{
		return weapons[index].gameObject.tag;
	}
}
