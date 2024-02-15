using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_IntroScene : MonoBehaviour
{
	[SerializeField]
	Button gameStartButton;

	private void Awake()
	{
		gameStartButton.onClick.AddListener(OnClickGameStartButton);
	}

	private void Start()
	{
		ResourceManager.Instance.LoadAllAsync("PreLoad", (key, count, totalCount) =>
		{
			// loadingSlider.value = (float)count/totalCount;
			// loadingValueText.text = $"{Math.Round(loadingSlider.value * 100)}%";
			if (count == totalCount)
			{
				// loadingSlider.value = 1;
				// loadingValueText.text = "100%";
				CompleteLoading();
			}
		});
	}


	// Start is called before the first frame update
	private void OnClickGameStartButton()
	{
		SceneManager.LoadScene("MainScene");
	}

	private void CompleteLoading()
	{
		//SoundManager.Instance.Play(SoundManager.SoundType.Effect, "Sound_IntroOkButton");

		TableLoader.Instance.Load();
		SceneManager.LoadScene("GameScene");

		//SceneChangeManager.Instance.LoadScene(Define.Scene.LobbyScene);
	}
}
