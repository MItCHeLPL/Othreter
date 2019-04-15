using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
	public GameObject loadGamePanel;
	public GameObject prevMenu;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			BackButton();
		}
	}

	public void BackButton()
	{
		loadGamePanel.SetActive(false);
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
