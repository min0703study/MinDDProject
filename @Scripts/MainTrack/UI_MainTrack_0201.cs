using System;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class UI_MainTrack_0201 : UI_MainTrackBase
{
	[Header("Room GameObjects")]
	[SerializeField] Image livingRoomAImage;
	[SerializeField] Action clickEventCallback;
	
	[SerializeField] NumberLock lockA;
	[SerializeField] BlockLock blockLock;
	bool isUnlockedLockA;
	
	bool isUnlockedLockB;
	bool isUnlockedLockC;

	InventoryCell SelectedInventoryCell;
	protected override void InitAwake()
	{
		base.InitAwake();
		
		scriptNextButton.onClick.AddListener(OnClickScriptButton);
		popupNextButton.onClick.AddListener(OnClickPopupButton);
		getItemNextButton.onClick.AddListener(OnClickGetItemNextButton);
		popupPanel.SetActive(false);
	}

	void Start()
	{
		Bind();
				
		Refresh();
		RefreshInventoryList();
	}

	private void Bind()
	{
		GameManager.Instance.OnChangedStep += Refresh;
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
				}

				SelectedInventoryCell = inventoryCell;
				SelectedInventoryCell.SetSelected(true);
			});

			inventoryCell.Refresh();
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
			else if (dialog.Type == "show_background")
			{
				var roomSprite = ResourceManager.Instance.Load<Sprite>(dialog.RoomImageAsset);
				livingRoomAImage.sprite = roomSprite;

				popupPanel.SetActive(false);
				scriptPanel.SetActive(false);
				characterImage.gameObject.SetActive(false);
			}
		}
	}

	public override void OnClickRoomObject(ClickableRoomObject clickableRoomObject)
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
					if(inventorySlot.ItemTextId == "block_b") 
					{
						blockLock.OnWhiteBlock();
					}
					
					if(inventorySlot.ItemTextId == "block_a") 
					{
						blockLock.OnGreenBlock();
					}
					GameManager.Instance.UseItem(index);
					RefreshInventoryList();

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
		else if (clickEvent.EventType == "FocusZone")
		{
			FocusZone focusZone = Enum.Parse<FocusZone>(clickEvent.Text);
			MoveTo(CurrentRoom, focusZone);
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
	}

	public void OnClickScriptButton()
	{
		scriptPanel.SetActive(false);
	}

	public void OnClickPopupButton()
	{
		popupPanel.SetActive(false);
		clickEventCallback?.Invoke();
	}

	private void OnClickGetItemNextButton()
	{
		getItemPanel.SetActive(false);
	}

	public override void Clear()
	{
		base.Clear();
		GameManager.Instance.OnChangedStep -= Refresh;
		UIManager.Instance.CloseCoreUI(this);
	}
	
	
	private void CheckClearMission() 
	{
		//if()
	}
	

}
