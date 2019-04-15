using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	private float oldTimeScale;
	public GameObject pauseMenu;
	public GameObject pauseMainMenu;
	public GameObject fpsUI;
	public GameObject settingsMenu;

	private void Start()
    {
		oldTimeScale = Time.timeScale;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		fpsUI.SetActive(false);
		DisableAllMenus();
	}

	void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && Time.timeScale > 0.0f)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.Confined;
			oldTimeScale = Time.timeScale;
			Time.timeScale = 0.0f;
			pauseMenu.SetActive(true);
			pauseMainMenu.SetActive(true);
		}
		else if(Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0.0f)
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			Time.timeScale = oldTimeScale;
			DisableAllMenus();
		}
    }

	public void ResumeButton()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		Time.timeScale = oldTimeScale;
		DisableAllMenus();
	}

	public void LoadButton()
	{
		//are you sure?
		//add
	}

	public void SettingsButton()
	{
		pauseMainMenu.SetActive(false);
		settingsMenu.SetActive(true);
	}

	public void ExitToMianMenuButton()
	{
		SceneManager.LoadScene(sceneName: "MainMenu"); //MainMenu Scene
	}

	public void DevSettingsButton()
	{
		//temp
		if(fpsUI.activeInHierarchy == false)
		{
			fpsUI.SetActive(true);
		}
		else
		{
			fpsUI.SetActive(false);
		}
	}

	private void DisableAllMenus()
	{
		pauseMenu.SetActive(false);
		pauseMainMenu.SetActive(false);
		settingsMenu.SetActive(false);
	}
}
