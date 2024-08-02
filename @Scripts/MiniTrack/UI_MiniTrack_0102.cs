
using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class UI_MiniTrack_0102 : UI_MainTrackBase
{
	[Header("GameUI")]
	[SerializeField] Button backButton;
	
	Dictionary<string, bool> objectClickMissionDict = new Dictionary<string, bool>();
	
	

	protected override void InitAwake()
	{
		base.InitAwake();

		scriptNextButton.onClick.AddListener(OnClickScriptButton);
		popupNextButton.onClick.AddListener(OnClickPopupNextButton);
		backButton.onClick.AddListener(OnClickBackButton);
		
		objectClickMissionDict.Add("kitchen_refrigerator_top", false);
		objectClickMissionDict.Add("kitchen_refrigerator_bottom", false);
		
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
		var clickEvent = GameFlowTable.Instance.GetRoomObjectEvent(GameManager.Instance.CurrentSection.SectionAsset, clickableRoomObject.ObjectTextId);
		CheckObjectClickMission(clickableRoomObject.ObjectTextId);
		
		if (clickEvent.ActionType == "Event")
		{
		}
		else if (clickEvent.ActionType == "Popup")
		{
			ShowPopup(clickEvent.ObjectImageAsset, clickEvent.Text);
		}
		else if (clickEvent.ActionType == "talking_to_myself")
		{
			popupPanel.SetActive(false);
			scriptPanel.SetActive(true);
			characterImage.gameObject.SetActive(false);
			nameText.text = "선";

			scriptText.text = clickEvent.Text;
		}
		
	}
	
	public override void UpdateMissionState(string objectTextId, RoomObjectEventTriggerType eventState)
	{
		base.UpdateMissionState(objectTextId, eventState);
		CheckObjectClickMission(objectTextId);
	}
	
	public override void Clear()
	{
		base.Clear();
		GameManager.Instance.OnChangedStep -= Refresh;
		UIManager.Instance.CloseCoreUI(this);
	}
}
