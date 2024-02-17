using System;
using System.Collections;
using System.Collections.Generic;
using TableData;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class InventoryCell : MonoBehaviour
{
	public int index;

	private Action OnClickedButton;

	[SerializeField] Button inventoryButton;
	[SerializeField] Image itemImage;

	private void Awake()
	{

	}

	public void SetInfo(int index, Action onClickedButton)
	{
		this.index = index;
		OnClickedButton += onClickedButton;
	}

	public void Refresh()
	{
		InventorySlot inventorySlot = GameManager.Instance.Inventory[index];
		if (inventorySlot.IsBlank == false)
		{
			var item = GameFlowTable.Instance.GetItemById(inventorySlot.ItemTextId);
			Sprite sprite = ResourceManager.Instance.Load<Sprite>(item.ItemImageAsset);
			itemImage.sprite = sprite;
		}
	}
}
