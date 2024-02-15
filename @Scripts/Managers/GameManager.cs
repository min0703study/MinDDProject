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
	

	public UI_PopupBase currentPopup;
	

	protected override void init()
	{
		base.init();
		
		DOTween.Init();
		
		CurrentSectionIndex = 0;
		CurrentDialogueIndex = 0;
	}
	
	public void ToNextStep() 
	{
		CurrentDialogueIndex += 1;
		if(CurrentSection.Dialogues.Count <= CurrentDialogueIndex) 
		{
			currentPopup.ClosePopupUI();
			ToNextSection();
		} else 
		{
			currentPopup.Refresh();
		}
	}
	
	public void ToNextSection() 
	{
		CurrentSectionIndex += 1;
		CurrentSection = GameFlowTable.Instance.GetSectionById(CurrentSectionIndex);
		
		if(CurrentSection.SectionType == "Dialogue") 
		{
			currentPopup = UIManager.Instance.ShowPopupUI<UI_DialogueScript>();
		}
	}
	
	
	// Update is called once per frame
	void Update()
	{
		
	}
}
