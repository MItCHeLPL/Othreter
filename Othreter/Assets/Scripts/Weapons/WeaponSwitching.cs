using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
	CameraBaseController cameraBaseController;
	GameObject pauseMenu;
	PlayerController playerController;

	[SerializeField] private List<GameObject> weapons = new List<GameObject>();

	[SerializeField] private List<Transform> hand = new List<Transform>();

	[SerializeField] private List<Vector3> posOffset = new List<Vector3>();

	[SerializeField] private List<Vector3> rotOffset = new List<Vector3>();

	[SerializeField] private List<int> camId = new List<int>();

	private int index = 0;
	private int prevIndex = 1;

	bool weaponsHidden = true;
	bool isSwitching = false;

	void Start()
    {
		cameraBaseController = ObjectsMenager.instance.cam.GetComponent<CameraBaseController>();
		pauseMenu = ObjectsMenager.instance.pauseMenu;
		playerController = ObjectsMenager.instance.player.GetComponent<PlayerController>();

		for (int i = 0; i < weapons.Count; i++)
		{
			weapons[i].SetActive(false);
		}

		weapons[0].SetActive(true);
	}
	
    void Update()
    {
		if (isSwitching == false && DataHolder.playerState_Aiming == false && pauseMenu.activeInHierarchy == false && DataHolder.playerState_Controllable)
		{
			if (Input.GetKeyDown(DataHolder.HideWeapon) || Input.GetKeyDown(DataHolder.HideWeaponController))
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

			if (Input.GetKeyDown(DataHolder.LastWeapon))
			{
				SwitchWeapon(prevIndex);
			}

			if (Input.GetKeyDown(DataHolder.WeaponSlotUpController) && weaponsHidden == false)
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

			if (Input.GetKeyDown(DataHolder.WeaponSlotDownController) && weaponsHidden == false)
			{
				if (index == 1)
				{
					SwitchWeapon(weapons.Count - 1);
				}
				else
				{
					SwitchWeapon(index - 1);
				}
			}


			if (Input.GetKeyDown(DataHolder.WeaponSlot1))
			{
				SwitchWeapon(1);
				weaponsHidden = false;
			}

			if (Input.GetKeyDown(DataHolder.WeaponSlot2) && weapons.Count > 2)
			{
				SwitchWeapon(2);
				weaponsHidden = false;
			}

			if (Input.GetKeyDown(DataHolder.WeaponSlot3) && weapons.Count > 3)
			{
				SwitchWeapon(3);
				weaponsHidden = false;
			}

			if (Input.GetKeyDown(DataHolder.WeaponSlot4) && weapons.Count > 4)
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
		weapons[index].transform.parent = transform;

		if(camId[newIndex] != camId[index])
		{
			cameraBaseController.ChangeCamera(camId[newIndex]);
		}

		weapons[newIndex].SetActive(true);
		weapons[newIndex].transform.parent = hand[newIndex];
		weapons[newIndex].transform.localPosition = posOffset[newIndex];
		weapons[newIndex].transform.localEulerAngles = rotOffset[newIndex];

		playerController.RefreshSpeed();

		prevIndex = index;
		index = newIndex;

		StartCoroutine(Cooldown(0.75f));


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
