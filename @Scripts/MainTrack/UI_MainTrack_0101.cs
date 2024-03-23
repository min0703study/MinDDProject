using System;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

public enum Room
{
	SunRoomA,
	LivingRoomA,
	LivingRoomB,
	LivingRoomDrawer,
	Kitchen,
}

public class UI_MainTrack_0101 : UI_MainTrackBase
{
	[Header("Module")]
	[Header("GameUI")]
	[SerializeField] TextMeshProUGUI missionText;

	[Header("GetItem")]
	[SerializeField] GameObject getItemPanel;
	[SerializeField] Image getItemImage;
	[SerializeField] TextMeshProUGUI getItemText;
	[SerializeField] Button getItemNextButton;

	[Header("Room GameObjects")]
	[SerializeField] GameObject livingRoomA;
	[SerializeField] GameObject sunRoomA;
	[SerializeField] GameObject kitchen;
	[SerializeField] GameObject livingRoomB;
	[SerializeField] GameObject livingRoomDrawer;
	[SerializeField] GameObject inventoryListGO;
	[SerializeField] Image livingRoomAImage;
	[SerializeField] Action clickEventCallback;

	InventoryCell SelectedInventoryCell;
	protected override void Init()
	{
		base.Init();
		scriptNextButton.onClick.AddListener(OnClickNextButton);
		popupNextButton.onClick.AddListener(OnClickPopupButton);
		getItemNextButton.onClick.AddListener(OnClickGetItemNextButton);
		popupPanel.SetActive(false);

		Bind();
	}

	void Start()
	{
		Refresh();
		RefreshInventoryList();
	}

	private void Bind()
	{
		GameManager.Instance.OnChangedStep += Refresh;
	}

	public void OnClickNextButton()
	{
		scriptPanel.SetActive(false);
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
			var dialog = dialogues[GameManager.Instance.CurrentDetailFlowId];

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

	public void Move(string to)
	{
		livingRoomA.SetActive(false);
		livingRoomB.SetActive(false);
		livingRoomDrawer.SetActive(false);
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
			case "Living_Drawer":
				livingRoomDrawer.SetActive(true);
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
		else if (arrowTextId == "to_living_room_drawer")
		{
			Move("Living_Drawer");
		}
		else if (arrowTextId == "to_kitchen_arrow")
		{
			Move("Kitchen");
		}
	}

	public void Test(GameObject gameObject)
	{

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
				
				SelectedInventoryCell = null;
				SelectedInventoryCell.SetSelected(false);
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

}
