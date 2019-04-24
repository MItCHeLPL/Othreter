using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public GameObject mainMenu;
	public GameObject newGame;
	public GameObject loadGame;
	public GameObject settingsMenu;

	void Start()
	{
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
		newGame.SetActive(false);
		loadGame.SetActive(false);
		settingsMenu.SetActive(false);
		mainMenu.SetActive(true);
	}

	public void NewGameButton()
	{
		mainMenu.SetActive(false);
		newGame.SetActive(true);
	}

	public void LoadButton()
	{
		mainMenu.SetActive(false);
		loadGame.SetActive(true);
	}

	public void SettingsButton()
	{
		mainMenu.SetActive(false);
		settingsMenu.SetActive(true);
	}

	public void ExitGameButton()
	{
		//are you sure?
		Application.Quit();
	}

	public void DevSettingsButton()
	{
		//add
	}
}
