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
			var dialog = dialogues[GameManager.Instance.CurrentDetailFlowId];


			if( dialog.SoundEffectAsset != null &&  dialog.SoundEffectAsset != string.Empty) 
			{
				SoundManager.Instance.Play(SoundManager.SoundType.Effect, dialog.SoundEffectAsset, 0.5f);
			}
			
			AllPanelDisable();
			var roomSprite = ResourceManager.Instance.Load<Sprite>(dialog.RoomImageAsset);
			roomIamge.sprite = roomSprite;
			
			if (dialog.Type == "show_background")
			{
			}
			else if (dialog.Type == "show_character")
			{
				ShowCharacter(dialog.CharacterImageAsset);
			}
			else if (dialog.Type == "visual_sound_effect")
			{
				ShowVisualSoundEffectPanel(dialog.VisualSoundEffectAsset);
				ShowCharacter(dialog.CharacterImageAsset);
			}
			else if (dialog.Type == "talking_to_myself")
			{
				ShowDialogueBox(dialog.Text, dialog.CharacterKey);
			}
			else if (dialog.Type == "thinking")
			{
				//scriptPanel.SetActive(true);
				thinkingPanel.SetActive(true);
				ShowDialogueBox(dialog.Text, dialog.CharacterKey);
				ShowCharacter(dialog.CharacterImageAsset);
			}
			else if (dialog.Type == "talk")
			{
				characterMaskImage.enabled = true;
				ShowDialogueBox(dialog.Text, dialog.CharacterKey);
				ShowCharacter(dialog.CharacterImageAsset);
			}
			else if (dialog.Type == "listen")
			{
				characterMaskImage.enabled = false;
				ShowDialogueBox(dialog.Text, dialog.CharacterKey);
				ShowCharacter(dialog.CharacterImageAsset);
			}
			else if (dialog.Type == "popup")
			{
				ShowPopup(dialog.PopupImageAsset);
			}
			else if (dialog.Type == "choice")
			{
				ShowCharacter(dialog.CharacterImageAsset);
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
