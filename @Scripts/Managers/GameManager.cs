using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TableData;
using UnityEngine;

public class GameManager : BaseManager<GameManager>
{
	public Section CurrentSection;
	public int CurrentSectionIndex;
	public int CurrentDialogueIndex;

	public UI_CoreLayerBase currentCoreLayer;

	public Action OnChangedStep;

	protected override void init()
	{
		base.init();

		DOTween.Init();

		CurrentSectionIndex = 0;
		CurrentDialogueIndex = 0;
	}

	public void StartNewGame()
	{
		CurrentSectionIndex = 1;
		CurrentSection = GameFlowTable.Instance.GetSectionById(CurrentSectionIndex);
		currentCoreLayer = UIManager.Instance.ShowCoreLayerUI<UI_DialogueScript>();

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
		if (CurrentSection.SectionType == "Dialogue")
		{
			currentCoreLayer = UIManager.Instance.ShowCoreLayerUI<UI_DialogueScript>();
		}
		else if (CurrentSection.SectionType == "MiniTrack")
		{
			currentCoreLayer = UIManager.Instance.ShowCoreLayerUI<UI_MiniTrackBase>(CurrentSection.MiniTrack.MiniTrackAsset);
		}

	}

	// Update is called once per frame
	void Update()
	{

	}
}
