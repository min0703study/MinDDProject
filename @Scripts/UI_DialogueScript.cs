using System;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using TMPro;
public class UI_DialogueScript : UI_PopupBase
{
	[SerializeField]
	private TextMeshProUGUI nameText;
	
	[SerializeField]
	private TextMeshProUGUI scriptText;

	[SerializeField]
	private Image roomIamge;

	[SerializeField]
	private Image characterImage;
	
	[SerializeField]
	private Image characterMaskImage;
	
	[SerializeField]
	public Button nextButton;
	
	protected override void Init()
	{
		base.Init();
		scriptText.text = string.Empty;
		
		nextButton.onClick.AddListener(OnClickNextButton);
		Refresh();
	}

	public override void Refresh() 
	{
		var dialog = GameManager.Instance.CurrentSection.Dialogues[GameManager.Instance.CurrentDialogueIndex];
		
		
		// 방 배경 설정
		var roomSprite = ResourceManager.Instance.Load<Sprite>(dialog.RoomImageAsset);
		roomIamge.sprite = roomSprite;		
		
		// 대화 상대 설정
		if(dialog.CharacterKey != "") 
		{
			characterImage.gameObject.SetActive(true);
			var characterSprite = ResourceManager.Instance.Load<Sprite>(dialog.CharacterImageAsset);
			characterImage.sprite = characterSprite;	
			characterMaskImage.enabled = dialog.Type == "talk";
		} else 
		{
			characterImage.gameObject.SetActive(false);
		}
		

		nameText.text = dialog.CharacterKey;
		StartTypingAnimation(dialog.Text);
	}
	
	public void OnClickNextButton() 
	{
		GameManager.Instance.ToNextStep();
	}

	void StartTypingAnimation(string script)
	{
		DOTween.To(() => 0, x => scriptText.text = script.Substring(0, x), script.Length, script.Length * 0.1f);
	}
	
	void StartTypingAnimations(string script)
	{
		scriptText.DOText(script, script.Length * 0.1f);
	}

	// void StartTypingEffect2(string script, float duration = 1.0f)
	// {
	// 	var foundSpaceIndexes = new List<int>();
	// 	foundSpaceIndexes.Add(0);
	// 	for (int i = 0; i < script.Length; i++)
	// 	{
	// 		if (script[i] == ' ')
	// 			foundSpaceIndexes.Add(i + 1);
	// 	}
	// 	foundSpaceIndexes.Add(script.Length - 1);
		
	// 	Sequence mySequence = DOTween.Sequence();
	// 	for (int i = 0; i < foundSpaceIndexes.Count - 1; i++)
	// 	{
	// 		int startIndex = foundSpaceIndexes[i];
	// 		int length = foundSpaceIndexes[i + 1] - startIndex;
	// 		string splitScript = script.Substring(startIndex, length);
	// 		string frontScript = script.Substring(0, startIndex);

	// 		mySequence.Append(DOTween.To(() => 0, x => scriptText.text = frontScript + splitScript.Substring(0, x), length, length * 0.1f));
	// 		mySequence.AppendInterval(0.5f);
	// 	}
		
	// 	mySequence.Play();

	// }
}
