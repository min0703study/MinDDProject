using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TableData;
using UnityEngine;

public class InventorySlot
{
	public bool IsBlank { get; set; }
	public string ItemTextId { get; set; }
}

public class GameManager : BaseManager<GameManager>
{
	public Section CurrentSection;
	public int CurrentSectionIndex;
	public int CurrentDialogueIndex;

	public UI_CoreLayerBase currentCoreLayer;

	public const int INVENTORY_SIZE = 9;
	public InventorySlot[] Inventory { get; private set; } = new InventorySlot[INVENTORY_SIZE];

	public Action OnChangedStep;

	protected override void init()
	{
		base.init();

		DOTween.Init();

		CurrentSectionIndex = 0;
		CurrentDialogueIndex = 0;

		for (int i = 0; i < GameManager.INVENTORY_SIZE; i++)
		{
			Inventory[i] = new InventorySlot();
			Inventory[i].IsBlank = true;
		}
	}

	public void StartNewGame()
	{
		CurrentSectionIndex = 4;
		CurrentSection = GameFlowTable.Instance.GetSectionById(CurrentSectionIndex);

		// var catfood = GameFlowTable.Instance.GetItemById("cat_food");
		// Inventory[0].IsBlank = false;
		// Inventory[0].SlotItem = catfood;

		StartSection();
	}

	public void ToNextStep()
	{
		CurrentDialogueIndex += 1;

		if (CurrentDialogueIndex >= CurrentSection.Dialogues.Count)
		{
			CurrentDialogueIndex = 0;
			ToNextSection();
		}

		OnChangedStep?.Invoke();
	}

	public void ToNextSection()
	{
		CurrentSectionIndex += 1;
		CurrentSection = GameFlowTable.Instance.GetSectionById(CurrentSectionIndex);

		currentCoreLayer.Clear();
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
			currentCoreLayer = UIManager.Instance.ShowCoreLayerUI<UI_MiniTrackBase>(CurrentSection.SectionAsset);
		}
		else if (CurrentSection.SectionType == "MainTrack")
		{
			currentCoreLayer = UIManager.Instance.ShowCoreLayerUI<UI_MainTrackBase>(CurrentSection.SectionAsset);
		}
	}

	public void AddItem(string itemTextId)
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
