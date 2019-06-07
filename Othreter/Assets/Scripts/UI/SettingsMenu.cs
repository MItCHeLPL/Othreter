using UnityEngine;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
	[SerializeField]
	private GameObject settingsMenu;
	[SerializeField]
	private GameObject prevMenu;
	[SerializeField]
	private TextMeshProUGUI settingTitle;

	[SerializeField]
	private GameObject gameplayMenu;
	[SerializeField]
	private GameObject audioMenu;
	[SerializeField]
	private GameObject graphicsMenu;
	[SerializeField]
	private GameObject controllsMenu;
	[SerializeField]
	private GameObject bindButtons;

	[SerializeField]
	private GameObject inputSettings;
	private InputMenager inputMenager;

	private void Start()
	{
		inputMenager = inputSettings.GetComponent<InputMenager>();
		GameplayButton();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && inputMenager.waitingForKey == false && inputMenager.wait == false)
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
	}
}