using System;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using TMPro;

public class UI_DialogueScript : UI_CoreLayerBase
{
	[Header("Room")]
	[SerializeField] Image roomIamge;

	protected override void Init()
	{
		base.Init();
		scriptText.text = string.Empty;

		scriptNextButton.onClick.AddListener(OnClickNextButton);
		Bind();
	}

	private void Start()
	{
		Refresh();
	}

	private void Bind()
	{
		GameManager.Instance.OnChangedStep += Refresh;
	}

	public override void Refresh()
	{
		var dialogues = GameManager.Instance.CurrentSection.Dialogues;

		if (dialogues != null && dialogues.Count > 0)
		{
			var dialogueIndex = GameManager.Instance.CurrentDialogueIndex;
			var dialog = dialogues[dialogueIndex];

			// 방 배경 설정
			var roomSprite = ResourceManager.Instance.Load<Sprite>(dialog.RoomImageAsset);
			roomIamge.sprite = roomSprite;

			if (dialog.Type == "show_background")
			{
				popupPanel.SetActive(false);
				scriptPanel.SetActive(false);
				characterImage.gameObject.SetActive(false);
			}
			else if (dialog.Type == "talking_to_myself")
			{
				popupPanel.SetActive(false);
				scriptPanel.SetActive(true);
				characterImage.gameObject.SetActive(false);
				nameText.text = dialog.CharacterKey;

				StartTypingAnimation(dialog.Text);
			}
			else if (dialog.Type == "talk")
			{
				popupPanel.SetActive(false);
				scriptPanel.SetActive(true);
				nameText.text = dialog.CharacterKey;

				characterImage.gameObject.SetActive(true);
				characterMaskImage.enabled = false;
				var characterSprite = ResourceManager.Instance.Load<Sprite>(dialog.CharacterImageAsset);
				characterImage.sprite = characterSprite;

				StartTypingAnimation(dialog.Text);
			}
			else if (dialog.Type == "listen")
			{
				popupPanel.SetActive(false);
				scriptPanel.SetActive(true);
				nameText.text = dialog.CharacterKey;

				characterImage.gameObject.SetActive(true);
				characterMaskImage.enabled = true;
				var characterSprite = ResourceManager.Instance.Load<Sprite>(dialog.CharacterImageAsset);
				characterImage.sprite = characterSprite;

				StartTypingAnimation(dialog.Text);
			}
			else if (dialog.Type == "popup")
			{
				popupPanel.SetActive(true);
				scriptPanel.SetActive(false);
				var popupSprite = ResourceManager.Instance.Load<Sprite>(dialog.PopupImageAsset);
				popupImage.sprite = popupSprite;
			}
		}
	}

	public void OnClickNextButton()
	{
		GameManager.Instance.ToNextStep();
	}

	void StartTypingAnimation(string script)
	{
		DOTween.To(() => 0, x => scriptText.text = script.Substring(0, x), script.Length, script.Length * 0.1f);
	}

	void StartTypingAnimations(string script)
	{
		scriptText.DOText(script, script.Length * 0.1f);
	}

	public override void Clear()
	{
		base.Clear();

		UIManager.Instance.CloseCoreUI(this);
	}

	// void StartTypingEffect2(string script, float duration = 1.0f)
	// {
	// 	var foundSpaceIndexes = new List<int>();
	// 	foundSpaceIndexes.Add(0);
	// 	for (int i = 0; i < script.Length; i++)
	// 	{
	// 		if (script[i] == ' ')
	// 			foundSpaceIndexes.Add(i + 1);
	// 	}
	// 	foundSpaceIndexes.Add(script.Length - 1);

	// 	Sequence mySequence = DOTween.Sequence();
	// 	for (int i = 0; i < foundSpaceIndexes.Count - 1; i++)
	// 	{
	// 		int startIndex = foundSpaceIndexes[i];
	// 		int length = foundSpaceIndexes[i + 1] - startIndex;
	// 		string splitScript = script.Substring(startIndex, length);
	// 		string frontScript = script.Substring(0, startIndex);

	// 		mySequence.Append(DOTween.To(() => 0, x => scriptText.text = frontScript + splitScript.Substring(0, x), length, length * 0.1f));
	// 		mySequence.AppendInterval(0.5f);
	// 	}

	// 	mySequence.Play();

	// }

	// void StartTypingEffect2(string script, float duration = 1.0f)
	// {
	// 	var foundSpaceIndexes = new List<int>();
	// 	foundSpaceIndexes.Add(0);
	// 	for (int i = 0; i < script.Length; i++)
	// 	{
	// 		if (script[i] == ' ')
	// 			foundSpaceIndexes.Add(i + 1);
	// 	}
	// 	foundSpaceIndexes.Add(script.Length - 1);

	// 	Sequence mySequence = DOTween.Sequence();
	// 	for (int i = 0; i < foundSpaceIndexes.Count - 1; i++)
	// 	{
	// 		int startIndex = foundSpaceIndexes[i];
	// 		int length = foundSpaceIndexes[i + 1] - startIndex;
	// 		string splitScript = script.Substring(startIndex, length);
	// 		string frontScript = script.Substring(0, startIndex);

	// 		mySequence.Append(DOTween.To(() => 0, x => scriptText.text = frontScript + splitScript.Substring(0, x), length, length * 0.1f));
	// 		mySequence.AppendInterval(0.5f);
	// 	}

	// 	mySequence.Play();

	// }
}
