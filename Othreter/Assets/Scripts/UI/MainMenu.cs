using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public GameObject settingsMenu;
	public GameObject mainMenu;

	void Start()
	{
		Cursor.visible = true;
		settingsMenu.SetActive(false);
		mainMenu.SetActive(true);
	}

	public void NewGameButton()
	{
		SceneManager.LoadScene(1); //temp
		//add
	}

	public void LoadButton()
	{
		SceneManager.LoadScene(1);
		//add
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
		SceneManager.LoadScene(1); //temp
		//add
	}
}
