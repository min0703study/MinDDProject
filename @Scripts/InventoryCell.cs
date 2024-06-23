using System;
using System.Collections;
using System.Collections.Generic;
using TableData;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class InventoryCell : MonoBehaviour
{
	public int Index { get; private set; }

	public Action<InventoryCell> OnClickButtonAction { get; set; }

	[SerializeField] Button inventoryButton;
	[SerializeField] Image itemImage;
	[SerializeField] GameObject selectedPanel;

	private void Awake()
	{
		inventoryButton.onClick.AddListener(OnClickButton);
	}

	public void SetInfo(int index, Action<InventoryCell> onClickedButton)
	{
		this.Index = index;
		OnClickButtonAction += onClickedButton;
	}

	public void Refresh()
	{
		InventorySlot inventorySlot = GameManager.Instance.Inventory[Index];
		if (inventorySlot.IsBlank == false)
		{
			var item = GameFlowTable.Instance.GetItemById(inventorySlot.ItemTextId);
			Sprite sprite = ResourceManager.Instance.Load<Sprite>(item.ItemImageAsset);
			itemImage.sprite = sprite;
		}
	}

	public void SetSelected(bool value)
	{
		selectedPanel.SetActive(value);
	}

	public void OnClickButton()
	{
		OnClickButtonAction?.Invoke(this);
	}
}
