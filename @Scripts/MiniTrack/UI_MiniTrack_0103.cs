
using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class UI_MiniTrack_0103 : UI_MainTrackBase
{
	[Header("GameUI")]
	[SerializeField] TextMeshProUGUI missionText;
	[SerializeField] Button backButton;
	
	Dictionary<string, bool> objectClickMissionDict = new Dictionary<string, bool>();
	
	

	protected override void Init()
	{
		base.Init();

		scriptNextButton.onClick.AddListener(OnClickScriptButton);
		popupNextButton.onClick.AddListener(OnClickPopupNextButton);
		backButton.onClick.AddListener(OnClickBackButton);
		
		objectClickMissionDict.Add("entrance_lock_a", false);
		objectClickMissionDict.Add("entrance_lock_b", false);
		objectClickMissionDict.Add("entrance_lock_c", false);
		objectClickMissionDict.Add("entrance_door_view", false);
		objectClickMissionDict.Add("entrance_leaflet", false);
		objectClickMissionDict.Add("entrance_photo", false);
		
		backButton.gameObject.SetActive(false);
	}

	private void OnClickBackButton()
	{
		GameManager.Instance.ToNextStep();
	}

	private void Start()
	{
		Bind();
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
			visualSoundEffectPanel.SetActive(false);
			
			var dialog = dialogues[GameManager.Instance.CurrentDetailFlowId];

			if (dialog.Type == "mission")
			{
				popupPanel.SetActive(false);
				scriptPanel.SetActive(false);
				characterPanel.SetActive(false);

				missionText.text = dialog.MissionText;
			}
			else if (dialog.Type == "talking_to_myself")
			{
				popupPanel.SetActive(false);
				scriptPanel.SetActive(true);
				characterImage.gameObject.SetActive(false);
				nameText.text = dialog.CharacterKey;

				StartTypingAnimation(dialog.Text);
			}
		}
	}

	private void CheckObjectClickMission(string objectTextId) 
	{
		if(objectClickMissionDict.ContainsKey(objectTextId)) 
		{
			if(objectClickMissionDict[objectTextId] == false) 
			{
				objectClickMissionDict[objectTextId] = true;
				
				bool isMissionClickCompleted = true;
				foreach(var objectClickMission in objectClickMissionDict.Values) 
				{
					if(objectClickMission == false) 
					{
						isMissionClickCompleted = false;
						break;
					}
				}
				
				if(isMissionClickCompleted) 
				{
					backButton.gameObject.SetActive(true);
				}
			}
		}
	}
	
	private void OnClickScriptButton()
	{
		scriptPanel.SetActive(false);
	}

	private void OnClickPopupNextButton()
	{
		popupPanel.SetActive(false);
	}

	public override void OnClickRoomObject(ClickableRoomObject clickableRoomObject)
	{
		var clickEvent = GameFlowTable.Instance.GetObjectClickEvent(clickableRoomObject.ObjectTextId);
		CheckObjectClickMission(clickableRoomObject.ObjectTextId);
		
		if (clickEvent.EventType == "Event")
		{
		}
		else if (clickEvent.EventType == "Explain")
		{
			popupPanel.SetActive(true);
			var imageSprite = ResourceManager.Instance.Load<Sprite>(clickEvent.ObjectImageAsset);
			popupImage.sprite = imageSprite;
			popupText.text = clickEvent.Text;
		}
		else if (clickEvent.EventType == "talking_to_myself")
		{
			popupPanel.SetActive(false);
			scriptPanel.SetActive(true);
			characterImage.gameObject.SetActive(false);
			nameText.text = "선";

			scriptText.text = clickEvent.Text;
		}
		
	}
	
	public override void Clear()
	{
		base.Clear();
		GameManager.Instance.OnChangedStep -= Refresh;
		UIManager.Instance.CloseCoreUI(this);
	}
}