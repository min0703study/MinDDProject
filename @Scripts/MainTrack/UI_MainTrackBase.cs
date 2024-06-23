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
	
	[SerializeField] protected Action clickEventCallback;
	
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
			visualSoundEffectPanel.SetActive(false);
			
			var dialog = dialogues[GameManager.Instance.CurrentDetailFlowId];
			if (dialog.Type == "mission")
			{
				popupPanel.SetActive(false);
				scriptPanel.SetActive(false);
				characterPanel.SetActive(false);
				missionText.text = dialog.MissionText;
				
				RoomType startRoom = Enum.Parse<RoomType>(dialog.MissionStartRoom);
				MoveTo(startRoom);
			}
			else if (dialog.Type == "popup")
			{
				popupPanel.SetActive(true);
				popupTextPanel.SetActive(false);
				scriptPanel.SetActive(false);
				var popupSprite = ResourceManager.Instance.Load<Sprite>(dialog.PopupImageAsset);
				popupImage.sprite = popupSprite;

				clickEventCallback = () =>
				{
					GameManager.Instance.ToNextStep();
					clickEventCallback = null;
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
		var clickEvent = GameFlowTable.Instance.GetObjectClickEvent(clickableRoomObject.ObjectTextId);
		var objectTextId = clickableRoomObject.ObjectTextId;
		
		if(SelectedInventoryCell != null) 
		{
			bool isUsedItem = false;
			if(clickEvent.EventType == "UseItem") 
			{
				var index = SelectedInventoryCell.Index;
				InventorySlot inventorySlot = GameManager.Instance.Inventory[index];
				
				if (inventorySlot.ItemTextId == clickEvent.ItemTextId)
				{
					GameManager.Instance.UseItem(index);

					var usedItem = GameFlowTable.Instance.GetObjectClickEvent("use_item_" + objectTextId);
					var imageSprite = ResourceManager.Instance.Load<Sprite>(usedItem.ObjectImageAsset);
					
					popupImage.sprite = imageSprite;
					popupText.text = usedItem.Text;
					
					popupPanel.SetActive(true);
					popupTextPanel.SetActive(false);

					isUsedItem = true;
					clickableRoomObject.ChangeObjectTextId("used_item_" + objectTextId);
					clickEventCallback = () =>
					{
						GameManager.Instance.ToNextStep();
						clickEventCallback = null;
					};
					
					return;
				}
			}
			
			if(isUsedItem == false) 
			{
				popupPanel.SetActive(false);
				scriptPanel.SetActive(true);
				characterImage.gameObject.SetActive(false);
				nameText.text = "선";

				scriptText.text = "여기에 사용하는게 아닌가봐.";
				
				SelectedInventoryCell.SetSelected(false);
				SelectedInventoryCell = null;
			}
		}
		
		if (clickEvent.EventType == "Event")
		{
			if (clickEvent.ObjectTextId == "sun_room_door")
			{
				GameManager.Instance.ToNextStep();
			}
		}
		else if (clickEvent.EventType == "Explain")
		{
			popupPanel.SetActive(true);
			popupTextPanel.SetActive(true);
			var imageSprite = ResourceManager.Instance.Load<Sprite>(clickEvent.ObjectImageAsset);
			popupImage.sprite = imageSprite;
			popupText.text = clickEvent.Text;
		}
		else if (clickEvent.EventType == "GetItem")
		{
			popupPanel.SetActive(true);
			popupTextPanel.SetActive(true);
			var imageSprite = ResourceManager.Instance.Load<Sprite>(clickEvent.ObjectImageAsset);
			popupImage.sprite = imageSprite;
			popupText.text = clickEvent.Text;

			clickEventCallback = () =>
			{
				var getItemClickEvent = GameFlowTable.Instance.GetObjectClickEvent("get_item_" + objectTextId);
				if (getItemClickEvent != null)
				{
					popupPanel.SetActive(true);
					popupTextPanel.SetActive(true);
					var imageSprite = ResourceManager.Instance.Load<Sprite>(getItemClickEvent.ObjectImageAsset);
					popupImage.sprite = imageSprite;
					popupText.text = getItemClickEvent.Text;

					clickEventCallback = () =>
					{
						clickableRoomObject.ChangeObjectTextId("got_item_" + objectTextId);
						ShowGetItemPopup(clickEvent.ItemTextId);
						GameManager.Instance.AddItem(clickEvent.ItemTextId);
						RefreshInventoryList();
						clickEventCallback = null;
					};
				}
				else
				{
					clickableRoomObject.ChangeObjectTextId("got_item_" + objectTextId);
					ShowGetItemPopup(clickEvent.ItemTextId);
					GameManager.Instance.AddItem(clickEvent.ItemTextId);
					RefreshInventoryList();
					clickEventCallback = null;
				}
			};
		}
		else if (clickEvent.EventType == "UseItem")
		{
			popupPanel.SetActive(true);
			popupTextPanel.SetActive(true);
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
		else if (clickEvent.EventType == "FocusZone")
		{
			FocusZone focusZone = Enum.Parse<FocusZone>(clickEvent.Text);
			MoveTo(CurrentRoom, focusZone);
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
	
	public virtual void OnClickMoveArrow(RoomType moveToRoom, FocusZone moveToFocusZone) 
	{
		MoveTo(moveToRoom, moveToFocusZone);
	}
	
	public void OnClickPopupButton()
	{
		popupPanel.SetActive(false);
		clickEventCallback?.Invoke();
	}
	
	public void OnClickScriptButton()
	{
		scriptPanel.SetActive(false);
	}

	private void OnClickGetItemNextButton()
	{
		getItemPanel.SetActive(false);
	}
	
	public virtual void UpdateMissionState() 
	{
	}
	
	public void ShowPopup(string imageAssetKey) 
	{
		popupPanel.SetActive(true);
		popupTextPanel.SetActive(false);
		scriptPanel.SetActive(false);
		
		var popupSprite = ResourceManager.Instance.Load<Sprite>(imageAssetKey);
		popupImage.sprite = popupSprite;
	}
}
