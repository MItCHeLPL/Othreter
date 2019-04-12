using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LevelMenager : MonoBehaviour
{
	public GameObject LevelLoaderPanel;
	public TextMeshProUGUI text;
	public Slider slider;

	private void Start()
	{
		LevelLoaderPanel.SetActive(false);
	}

	public void LoadLevel(int index)
	{
		StartCoroutine(LoadAsynchronously(index));
	}

	private IEnumerator LoadAsynchronously(int index)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(index);

		LevelLoaderPanel.SetActive(true);

		while (operation.isDone == false)
		{
			float progress = Mathf.Clamp01(operation.progress / 0.9f);

			slider.value = progress;
			text.SetText("{0}%", progress * 100f);

			yield return null;
		}

		LevelLoaderPanel.SetActive(false);
	}
}
