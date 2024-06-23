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

	protected override void InitAwake()
	{
		base.InitAwake();
		scriptText.text = string.Empty;

		scriptNextButton.onClick.AddListener(OnClickNextButton);
		popupNextButton.onClick.AddListener(OnClickNextButton);
		roomNextButton.onClick.AddListener(OnClickNextButton);
		visualSoundEffectNextButton.onClick.AddListener(OnClickNextButton);

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
			choicePanel.SetActive(false);
			popupPanel.SetActive(false);
			scriptPanel.SetActive(false);
			thinkingPanel.SetActive(false);
			characterImage.gameObject.SetActive(false);
			characterMaskImage.enabled = false;
			visualSoundEffectPanel.SetActive(false);
				
			var dialog = dialogues[GameManager.Instance.CurrentDetailFlowId];

			var roomSprite = ResourceManager.Instance.Load<Sprite>(dialog.RoomImageAsset);
			roomIamge.sprite = roomSprite;
			
			if( dialog.SoundEffectAsset != null &&  dialog.SoundEffectAsset != string.Empty) 
			{
				SoundManager.Instance.Play(SoundManager.SoundType.Effect, dialog.SoundEffectAsset, 0.5f);
			}


			if (dialog.Type == "show_background")
			{
			}
			else if (dialog.Type == "show_character")
			{
				characterPanel.SetActive(true);
				characterImage.gameObject.SetActive(true);

				nameText.text = dialog.CharacterKey;
				
				var characterSprite = ResourceManager.Instance.Load<Sprite>(dialog.CharacterImageAsset);
				characterImage.sprite = characterSprite;
			}
			else if (dialog.Type == "visual_sound_effect")
			{
				visualSoundEffectPanel.SetActive(true);

				nameText.text = dialog.CharacterKey;
				visualSoundEffectImage.sprite =  ResourceManager.Instance.Load<Sprite>(dialog.VisualSoundEffectAsset);
				
				if(dialog.CharacterKey != null && dialog.CharacterKey != string.Empty) 
				{
					characterPanel.SetActive(true);
					characterImage.gameObject.SetActive(true);

					nameText.text = "선";
					
					var characterSprite = ResourceManager.Instance.Load<Sprite>(dialog.CharacterImageAsset);
					characterImage.sprite = characterSprite;
				}
			}
			else if (dialog.Type == "talking_to_myself")
			{
				scriptPanel.SetActive(true);
				nameText.text = dialog.CharacterKey;

				StartTypingAnimation(dialog.Text);
			}
			else if (dialog.Type == "thinking")
			{
				scriptPanel.SetActive(true);
				thinkingPanel.SetActive(true);
				
				if(dialog.CharacterKey != null && dialog.CharacterKey != string.Empty) 
				{
					characterPanel.SetActive(true);
					characterImage.gameObject.SetActive(true);

					nameText.text = "선";
					
					var characterSprite = ResourceManager.Instance.Load<Sprite>(dialog.CharacterImageAsset);
					characterImage.sprite = characterSprite;
				}

				StartTypingAnimation(dialog.Text);
			}
			else if (dialog.Type == "talk")
			{
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
				scriptPanel.SetActive(true);
				characterPanel.SetActive(true);
				characterImage.gameObject.SetActive(true);
				
				nameText.text = dialog.CharacterKey;

				characterMaskImage.enabled = false;
				var characterSprite = ResourceManager.Instance.Load<Sprite>(dialog.CharacterImageAsset);
				characterImage.sprite = characterSprite;

				StartTypingAnimation(dialog.Text);
			}
			else if (dialog.Type == "popup")
			{
				popupPanel.SetActive(true);
				var popupSprite = ResourceManager.Instance.Load<Sprite>(dialog.PopupImageAsset);
				popupImage.sprite = popupSprite;
			}
			else if (dialog.Type == "choice")
			{
				choicePanel.SetActive(true);
				
				Util.DestroyChilds(choiceListPanel);
				string[] choiceItems = dialog.Text.Split(",");
				
				int index = 1;
				foreach(string text in choiceItems) 
				{
					var itemHolderGO = Instantiate(choiceItemHolderPrefab);
					itemHolderGO.transform.SetParent(choiceListPanel.transform, false);
					var itemHolder = Util.GetOrAddComponent<ChoiceItemHolder>(itemHolderGO);
					itemHolder.Init(text, index, (int index) => 
					{
						GameManager.Instance.ChoiceIndex(index);
						GameManager.Instance.ToNextStep();
					});
					
					index++;
				}
				
				if(dialog.CharacterKey != null && dialog.CharacterKey != string.Empty) 
				{
					characterPanel.SetActive(true);
					characterImage.gameObject.SetActive(true);

					var characterSprite = ResourceManager.Instance.Load<Sprite>(dialog.CharacterImageAsset);
					characterImage.sprite = characterSprite;
				}
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
