using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class RoomWrapper
{
	public RoomType Room;
	public RoomController FocusZoneGO;
}

public class UI_MainTrackBase : UI_CoreLayerBase
{
	[SerializeField] private UI_TrackPanel trackPanel;
	
	protected RoomType CurrentRoom { get; set; } = RoomType.None;
	FocusZone CurrentFocusZone { get; set; }
	
	[SerializeField] List<RoomController> rooms = new ();
	public Dictionary<RoomType, RoomController> RoomDict = new ();
	
	[SerializeField] public Action OnClickPopupNextButton;
	
	InventoryCell SelectedInventoryCell;
		
	#region UI Components
	protected TextMeshProUGUI missionText => trackPanel.missionText;
	protected GameObject inventoryListGO => trackPanel.inventoryListGO;
	protected GameObject getItemPanel => trackPanel.getItemPanel;
	protected Image getItemImage => trackPanel.getItemImage;
	protected TextMeshProUGUI getItemText => trackPanel.getItemText;
	protected Button getItemNextButton => trackPanel.getItemNextButton;
	
	#endregion
	
	void Start()
	{
		Bind();
				
		Refresh();
		RefreshInventoryList();
	}

	private void Bind()
	{
		GameManager.Instance.OnChangedStep += Refresh;
		GameManager.Instance.OnUseInventoryItem += RefreshInventoryList;
		GameManager.Instance.OnAddInventoryItem += PerformAddInventoryItem;
	}

	public override void Clear()
	{
		base.Clear();
		
		GameManager.Instance.OnChangedStep -= Refresh;
		GameManager.Instance.OnUseInventoryItem -= RefreshInventoryList;
		GameManager.Instance.OnAddInventoryItem -= PerformAddInventoryItem;
		UIManager.Instance.CloseCoreUI(this);
	}

	
	protected override void InitAwake()
	{
		base.InitAwake();
		
		scriptNextButton.onClick.AddListener(OnClickScriptButton);
		popupNextButton.onClick.AddListener(OnClickPopupButton);
		getItemNextButton.onClick.AddListener(OnClickGetItemNextButton);
		
		foreach(var room in rooms) 
		{
			RoomDict.Add(room.Room, room);
		}
		
		foreach(var room in rooms) 
		{
			room.Initialize();
			foreach(var focusZoneGO in room.FocusZoneDict.Values) 
			{
				focusZoneGO.SetActive(false);
			}
			
			room.gameObject.SetActive(false);
		};
		
		popupPanel.SetActive(false);
	}
	
	public override void Refresh()
	{
		var dialogues = GameManager.Instance.CurrentSection.Dialogues;

		if (dialogues != null && dialogues.Count > 0)
		{
			AllPanelDisable();
			
			var dialog = dialogues[GameManager.Instance.CurrentDetailFlowId];
			if (dialog.Type == "mission")
			{
				missionText.text = dialog.MissionText;
				
				RoomType startRoom = Enum.Parse<RoomType>(dialog.MissionStartRoom);
				MoveTo(startRoom);
			}
			else if (dialog.Type == "popup")
			{
				ShowPopup(dialog.PopupImageAsset, dialog.Text);
				OnClickPopupNextButton = () =>
				{
					GameManager.Instance.ToNextStep();
					OnClickPopupNextButton = null;
				};
			}
		}
	}

	
	public void PerformAddInventoryItem(string itemTextId) 
	{
		ShowGetItemPopup(itemTextId);
		RefreshInventoryList();
	}

	private void RefreshInventoryList()
	{
		Util.DestroyChilds(inventoryListGO);
		for (int i = 0; i < GameManager.INVENTORY_SIZE; i++)
		{
			var inventoryCell = UIManager.Instance.MakeSubItem<InventoryCell>(inventoryListGO.transform);
			inventoryCell.SetInfo(i, (inventoryCell) =>
			{
				InventorySlot inventorySlot = GameManager.Instance.Inventory[inventoryCell.Index];
				if(inventorySlot.IsBlank) 
				{
					return;
				}
				
				if (SelectedInventoryCell != null)
				{
					SelectedInventoryCell.SetSelected(false);
					GameManager.Instance.SelectedInventoryIndex = -1;
				}

				SelectedInventoryCell = inventoryCell;
				SelectedInventoryCell.SetSelected(true);
				
				GameManager.Instance.SelectedInventoryIndex = inventoryCell.Index;
			});
			
			if(GameManager.Instance.SelectedInventoryIndex == i) 
			{
				SelectedInventoryCell = inventoryCell;
				SelectedInventoryCell.SetSelected(true);
			}
			
			inventoryCell.Refresh();
		}
	}

	public virtual void OnClickRoomObject(ClickableRoomObject clickableRoomObject)
	{
		var clickEvent = GameFlowTable.Instance.GetRoomObjectEvent(clickableRoomObject.ObjectTextId);
		UpdateMissionState(clickableRoomObject.ObjectTextId, RoomObjectEventTriggerType.Click);
		if (clickEvent.ActionType == "Event")
		{
			if (clickEvent.ObjectTextId == "sun_room_door")
			{
				GameManager.Instance.ToNextStep();
			}
		}
		else if (clickEvent.ActionType == "Popup")
		{
			ShowPopup(clickEvent.ObjectImageAsset, clickEvent.Text);
		}
		else if (clickEvent.ActionType == "talking_to_myself")
		{
			
			ShowDialogueBox(clickEvent.Text, "선", false);
			popupPanel.SetActive(false);
			scriptPanel.SetActive(true);
			characterImage.gameObject.SetActive(false);
			nameText.text = "선";

			scriptText.text = clickEvent.Text;
		}
	}

	public void ShowGetItemPopup(string itemTextId)
	{
		var item = GameFlowTable.Instance.GetItemById(itemTextId);
		var imageSprite = ResourceManager.Instance.Load<Sprite>(item.ItemImageAsset);
		getItemImage.sprite = imageSprite;
		getItemText.text = $"[<color=yellow>{item.ItemName}</color=yellow>]을 발견했다!";
		getItemPanel.SetActive(true);
	}
	
	public void MoveTo(RoomType moveToRoom, FocusZone moveToFocusZone = FocusZone.None)
	{
		if(moveToFocusZone == FocusZone.None) 
		{
			if(Enum.TryParse($"{moveToRoom}_Full", out FocusZone focusZone))
			{
				moveToFocusZone = focusZone; 
			} else 
			{
				Debug.LogError($"Not Found Focus Zone {$"{moveToRoom}_Full"}");
				return;
			}
		}
		
		if(CurrentRoom != RoomType.None) 
		{
			RoomDict[CurrentRoom].gameObject.SetActive(false);
			RoomDict[CurrentRoom].FocusZoneDict[CurrentFocusZone].SetActive(false);
		}
		
		RoomDict[moveToRoom].gameObject.SetActive(true);
		RoomDict[moveToRoom].FocusZoneDict[moveToFocusZone].SetActive(true);
		
		CurrentRoom = moveToRoom;
		CurrentFocusZone = moveToFocusZone;
	}
	
	public void PerformClickInRoomItem(LocatedInRoomItem roomItem) 
	{
		GameManager.Instance.AddItem(roomItem.ItemTextId);
	}
	
	
	public virtual void OnClickMoveArrow(RoomType moveToRoom, FocusZone moveToFocusZone) 
	{
		MoveTo(moveToRoom, moveToFocusZone);
	}
	
	public void OnClickPopupButton()
	{
		popupPanel.SetActive(false);
		OnClickPopupNextButton?.Invoke();
	}
	
	public void OnClickScriptButton()
	{
		scriptPanel.SetActive(false);
	}

	private void OnClickGetItemNextButton()
	{
		getItemPanel.SetActive(false);
	}
	
	public virtual void UpdateMissionState(string objectTextId, RoomObjectEventTriggerType eventState) 
	{
		
	}
}
