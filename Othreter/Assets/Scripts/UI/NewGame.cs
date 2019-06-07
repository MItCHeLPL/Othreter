using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour
{
	[SerializeField]
	private GameObject newGamePanel;
	[SerializeField]
	private GameObject prevMenu;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			BackButton();
		}
	}

	public void BackButton()
	{
		newGamePanel.SetActive(false);
		prevMenu.SetActive(true);
	}

	public void SaveFile1Button()
	{
		//add
	}

	public void SaveFile2Button()
	{
		//add
	}

	public void SaveFile3Button()
	{
		//add
	}
}
