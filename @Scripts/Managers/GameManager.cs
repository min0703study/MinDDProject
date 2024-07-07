using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TableData;
using UnityEditor.Search;
using UnityEngine;

public class InventorySlot
{
	public bool IsBlank { get; set; }
	public string ItemTextId { get; set; }
}

public class GameManager : BaseManager<GameManager>
{
	public Section CurrentSection;
	public int CurrentGameFlowIndex;
	public int CurrentDetailFlowIndex;
	public int CurrentChoiceIndex;
	public int CurrentDetailChoiceIndex;
	public string CurrentDetailFlowId;

	public UI_CoreLayerBase currentCoreLayer;

	public const int INVENTORY_SIZE = 9;
	public InventorySlot[] Inventory { get; private set; } = new InventorySlot[INVENTORY_SIZE];
	public int SelectedInventoryIndex { get; set; } = 0;
	public Action OnChangedStep;
	public Action OnUseInventoryItem;
	public Action<string> OnAddInventoryItem;
	
	public Dictionary<string, string> AllItems;
	
	protected override void init()
	{
		base.init();

		DOTween.Init();

		for (int i = 0; i < GameManager.INVENTORY_SIZE; i++)
		{
			Inventory[i] = new InventorySlot();
			Inventory[i].IsBlank = true;
		}
	}

	public void StartNewGame()
	{
		CurrentGameFlowIndex = 13;
		
		CurrentDetailFlowIndex = 1;
		CurrentDetailFlowId = "1";
		CurrentChoiceIndex = -1;
		
		CurrentSection = GameFlowTable.Instance.GetSectionById(CurrentGameFlowIndex);

		StartSection();
	}

	public void ChoiceIndex(int index) 
	{
		CurrentChoiceIndex = index;
		CurrentDetailChoiceIndex = 0;
	}
	
	public void ToNextStep()
	{
		if(CurrentChoiceIndex != -1) 
		{
			CurrentDetailChoiceIndex += 1;
			CurrentDetailFlowId = $"{CurrentDetailFlowIndex}-{CurrentChoiceIndex}-{CurrentDetailChoiceIndex}";
			
			if (!CurrentSection.Dialogues.ContainsKey(CurrentDetailFlowId))
			{
				CurrentChoiceIndex = -1;
				CurrentDetailFlowIndex += 1;
				CurrentDetailFlowId = $"{CurrentDetailFlowIndex}";
				
				if (!CurrentSection.Dialogues.ContainsKey(CurrentDetailFlowId))
				{
					CurrentDetailFlowIndex = 1;
					CurrentDetailFlowId = $"1";
					ToNextSection();
				}
			}
		} else 
		{
			CurrentDetailFlowIndex += 1;
			CurrentDetailFlowId = $"{CurrentDetailFlowIndex}";
			
			if (!CurrentSection.Dialogues.ContainsKey(CurrentDetailFlowId))
			{
				CurrentDetailFlowIndex = 1;
				CurrentDetailFlowId = $"1";
				ToNextSection();
			}
		}

		OnChangedStep?.Invoke();
	}

	public void ToNextSection()
	{
		CurrentGameFlowIndex += 1;
		CurrentSection = GameFlowTable.Instance.GetSectionById(CurrentGameFlowIndex);

		currentCoreLayer.Clear();
		currentCoreLayer = null;

		StartSection();
	}

	public void StartSection()
	{
		if (CurrentSection.SectionType == "Dialogue")
		{
			currentCoreLayer = UIManager.Instance.ShowCoreLayerUI<UI_DialogueScript>();
		}
		else if (CurrentSection.SectionType == "MiniTrack")
		{
			currentCoreLayer = UIManager.Instance.ShowCoreLayerUI<UI_MainTrackBase>(CurrentSection.SectionAsset);
		}
		else if (CurrentSection.SectionType == "MainTrack")
		{
			currentCoreLayer = UIManager.Instance.ShowCoreLayerUI<UI_MainTrackBase>(CurrentSection.SectionAsset);
		}
		else if (CurrentSection.SectionType == "MiniGame")
		{
			currentCoreLayer = UIManager.Instance.ShowCoreLayerUI<UI_CoreLayerBase>(CurrentSection.SectionAsset);
		}
		else if (CurrentSection.SectionType == "ChapterTitle")
		{
			currentCoreLayer = UIManager.Instance.ShowCoreLayerUI<UI_CoreLayerBase>(CurrentSection.SectionAsset);
		}
	}

	public void AddItem(string itemTextId)
	{
		for (int i = 0; i < GameManager.INVENTORY_SIZE; i++)
		{

			if (Inventory[i].IsBlank)
			{
				Inventory[i].IsBlank = false;
				Inventory[i].ItemTextId = itemTextId;
				break;
			};
		}
		
		OnAddInventoryItem?.Invoke(itemTextId);
	}

	public void UseItem(int index)
	{
		Inventory[index].IsBlank = true;
		Inventory[index].ItemTextId = string.Empty;
		
		OnUseInventoryItem?.Invoke();
	}

	public string GetSelectedInventoryItemId()
	{
		if(SelectedInventoryIndex != -1) 
		{
			InventorySlot slot = Inventory[SelectedInventoryIndex];
			return slot.ItemTextId;
		}	
		
		return string.Empty;
	}
}
