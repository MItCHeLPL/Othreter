using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	private float oldTimeScale;
	[SerializeField]
	private GameObject pauseMenu;
	[SerializeField]
	private GameObject pauseMainMenu;
	[SerializeField]
	private GameObject fpsUI;
	[SerializeField]
	private GameObject settingsMenu;

	[SerializeField]
	private GameObject inputSettings;
	private InputMenager inputMenager;
	private void Start()
    {
		inputMenager = inputSettings.GetComponent<InputMenager>();
		oldTimeScale = Time.timeScale;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		fpsUI.SetActive(false);
		DisableAllMenus();
	}

	void Update()
    {
        if((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(DataHolder.PauseController)) && Time.timeScale > 0.0f)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.Confined;
			oldTimeScale = Time.timeScale;
			Time.timeScale = 0.0f;
			pauseMenu.SetActive(true);
			pauseMainMenu.SetActive(true);
		}
		else if((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(DataHolder.PauseController) || Input.GetKeyDown(DataHolder.BackController)) && inputMenager.waitingForKey == false && inputMenager.wait == false && Time.timeScale == 0.0f)
		{
			ResumeButton();
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
