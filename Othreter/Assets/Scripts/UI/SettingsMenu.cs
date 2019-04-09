using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
	public GameObject settingsMenu;
	public GameObject prevMenu;
	public TextMeshProUGUI settingTitle;

	public GameObject gameplayMenu;
	public GameObject audioMenu;
	public GameObject graphicsMenu;
	public GameObject controllsMenu;
	public GameObject bindButtons;

	public void Start()
	{
		GameplayButton();
	}

	public void BackButton()
	{
		settingsMenu.SetActive(false);
		prevMenu.SetActive(true);
	}

	public void GameplayButton()
	{
		audioMenu.SetActive(false);
		controllsMenu.SetActive(false);
		graphicsMenu.SetActive(false);

		gameplayMenu.SetActive(true);
		settingTitle.SetText("Gameplay Settings");
	}

	public void AudioButton()
	{
		gameplayMenu.SetActive(false);
		controllsMenu.SetActive(false);
		graphicsMenu.SetActive(false);

		audioMenu.SetActive(true);
		settingTitle.SetText("Audio Settings");
	}

	public void GraphicsButton()
	{
		audioMenu.SetActive(false);
		gameplayMenu.SetActive(false);
		controllsMenu.SetActive(false);

		graphicsMenu.SetActive(true);
		settingTitle.SetText("Graphics Settings");
	}

	public void ContollsButton()
	{
		audioMenu.SetActive(false);
		gameplayMenu.SetActive(false);
		graphicsMenu.SetActive(false);

		controllsMenu.SetActive(true);
		settingTitle.SetText("Contolls Settings");

		GameObject.FindGameObjectWithTag("GameSettings").GetComponent<InputMenager>().RenameControllButtons(bindButtons);
	}
}