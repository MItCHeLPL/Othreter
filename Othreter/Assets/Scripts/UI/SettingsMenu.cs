using UnityEngine;
using TMPro;

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

	private void Start()
	{
		GameplayButton();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && InputMenager.input.waitingForKey == false && InputMenager.input.wait == false)
		{
			BackButton();
		}
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

		InputMenager.input.RenameControllButtons(bindButtons);
	}
}