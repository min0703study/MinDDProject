using System;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using TMPro;

public class UI_DialogueScript : UI_CoreLayerBase
{
	[Header("Room")]
	[SerializeField] Image roomIamge;
	[SerializeField] Button roomNextButton;

	protected override void Init()
	{
		base.Init();
		scriptText.text = string.Empty;

		scriptNextButton.onClick.AddListener(OnClickNextButton);
		popupNextButton.onClick.AddListener(OnClickNextButton);
		roomNextButton.onClick.AddListener(OnClickNextButton);

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
				characterPanel.SetActive(true);

				nameText.text = dialog.CharacterKey;

				characterImage.gameObject.SetActive(true);
				characterMaskImage.enabled = true;
				var characterSprite = ResourceManager.Instance.Load<Sprite>(dialog.CharacterImageAsset);
				characterImage.sprite = characterSprite;

				StartTypingAnimation(dialog.Text);
			}
			else if (dialog.Type == "listen")
			{
				popupPanel.SetActive(false);
				scriptPanel.SetActive(true);
				characterPanel.SetActive(true);

				nameText.text = dialog.CharacterKey;

				characterImage.gameObject.SetActive(true);
				characterMaskImage.enabled = false;
				var characterSprite = ResourceManager.Instance.Load<Sprite>(dialog.CharacterImageAsset);
				characterImage.sprite = characterSprite;

				StartTypingAnimation(dialog.Text);
			}
			else if (dialog.Type == "popup")
			{
				popupPanel.SetActive(true);
				scriptPanel.SetActive(false);
				popupTextPanel.SetActive(false);
				var popupSprite = ResourceManager.Instance.Load<Sprite>(dialog.PopupImageAsset);
				popupImage.sprite = popupSprite;
			}
		}
	}

	public void OnClickNextButton()
	{
		GameManager.Instance.ToNextStep();
	}

	public override void Clear()
	{
		base.Clear();
		GameManager.Instance.OnChangedStep -= Refresh;
		UIManager.Instance.CloseCoreUI(this);
	}
}
