using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectsMenager : MonoBehaviour
{
    #region Singleton

    public static ObjectsMenager instance;

    void Awake()
    {
        instance = this;
    }

	#endregion

	[Header("Player")]
	public GameObject player;
	public GameObject playerModel;
	public Camera cam;
	public GameObject weaponHolder;
	public GameObject bow;
	public GameObject sword;

	[Header("UI")]
	public GameObject UIMenager;
	public GameObject ammoUI;
	public GameObject healthUI;
	public GameObject pauseMenu;
	public GameObject bowCrosshair;
	public GameObject swordCrosshair;
	public GameObject fpsUI;
}