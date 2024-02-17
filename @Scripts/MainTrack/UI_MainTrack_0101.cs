using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public enum Room
{
	SunRoomA,
	LivingRoomA,
	LivingRoomB,
	Kitchen,
}

public class UI_MainTrack_0101 : UI_MainTrackBase
{
	[Header("Module")]
	[Header("GameUI")]
	[SerializeField] TextMeshProUGUI missionText;

	[Header("Room GameObjects")]
	[SerializeField] GameObject livingRoomA;
	[SerializeField] GameObject sunRoomA;
	[SerializeField] GameObject kitchen;
	[SerializeField] GameObject livingRoomB;

	[SerializeField] GameObject inventoryListGO;

	private void Awake()
	{
		popupNextButton.onClick.AddListener(OnClickPopupButton);
		popupPanel.SetActive(false);
	}
	// Start is called before the first frame update
	void Start()
	{
		Refresh();
		RefreshInventoryList();
	}

	private void RefreshInventoryList()
	{
		Util.DestroyChilds(inventoryListGO);
		for (int i = 0; i < GameManager.INVENTORY_SIZE; i++)
		{
			var inventoryCell = UIManager.Instance.MakeSubItem<InventoryCell>(inventoryListGO.transform);
			inventoryCell.SetInfo(i, null);
			inventoryCell.Refresh();
		}
	}

	public override void Refresh()
	{
		var dialogues = GameManager.Instance.CurrentSection.Dialogues;

		if (dialogues != null && dialogues.Count > 0)
		{
			var dialogueIndex = GameManager.Instance.CurrentDialogueIndex;
			var dialog = dialogues[dialogueIndex];

			if (dialog.Type == "mission")
			{
				popupPanel.SetActive(false);
				scriptPanel.SetActive(false);
				characterPanel.SetActive(false);
				missionText.text = dialog.MissionText;

				Move(dialog.MissionStartRoom);
			}
			else if (dialog.Type == "talking_to_myself")
			{
				popupPanel.SetActive(false);
				scriptPanel.SetActive(true);
				characterImage.gameObject.SetActive(false);
				nameText.text = dialog.CharacterKey;

				StartTypingAnimation(dialog.Text);
			}
			else if (dialog.Type == "popup")
			{
				popupPanel.SetActive(true);
				scriptPanel.SetActive(false);
				var popupSprite = ResourceManager.Instance.Load<Sprite>(dialog.PopupImageAsset);
				popupImage.sprite = popupSprite;

				popupNextButton.onClick.AddListener(OnClickNextStep);
			}
		}
	}

	public void Move(string to)
	{
		livingRoomA.SetActive(false);
		livingRoomB.SetActive(false);
		sunRoomA.SetActive(false);
		kitchen.SetActive(false);

		switch (to)
		{
			case "Living_A":
				livingRoomA.SetActive(true);
				break;
			case "Living_B":
				livingRoomB.SetActive(true);
				break;
			case "Sun_A":
				sunRoomA.SetActive(true);
				break;
			case "Kitchen":
				kitchen.SetActive(true);
				break;
		}
	}

	public void OnClickArrowButton(string arrowTextId)
	{
		if (arrowTextId == "to_living_room_a_arrow")
		{
			Move("Living_A");
		}
		else if (arrowTextId == "to_living_room_b_arrow")
		{
			Move("Living_B");
		}
		else if (arrowTextId == "to_kitchen_arrow")
		{
			Move("Kitchen");
		}
	}

	public void OnClickObject(string objectTextId)
	{
		var clickEvent = GameFlowTable.Instance.GetObjectClickEvent(objectTextId);
		if (clickEvent.EventType == "Event")
		{
			if (clickEvent.ObjectTextId == "sun_room_door")
			{
				//Move(Room.LivingRoomA);
				GameManager.Instance.ToNextStep();
				Refresh();
			}
		}
		else if (clickEvent.EventType == "Explain")
		{
			popupPanel.SetActive(true);
			var imageSprite = ResourceManager.Instance.Load<Sprite>(clickEvent.ObjectImageAsset);
			popupImage.sprite = imageSprite;
			popupText.text = clickEvent.Text;
		}
	}

	public void OnClickPopupButton()
	{
		popupPanel.SetActive(false);
	}

	public void OnClickNextStep()
	{
		GameManager.Instance.ToNextStep();
		popupNextButton.onClick.RemoveListener(OnClickNextStep);
		Refresh();
	}

}
