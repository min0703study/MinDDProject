using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MiniTrack_0101 : UI_MiniTrackBase
{
	[Header("Modules")]
	[Header("Cellphone")]

	[SerializeField]
	GameObject cellphonePanel;

	[SerializeField]
	Button lockButton;

	[SerializeField]
	Image roomImage;


	private int currentDialogueIndex;

	// Start is called before the first frame update
	void Start()
	{
		scriptNextButton.onClick.AddListener(OnClickScriptButton);
		lockButton.onClick.AddListener(OnClickLockButton);

		scriptPanel.SetActive(false);
		cellphonePanel.SetActive(false);

		currentDialogueIndex = 0;
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnClickLockButton()
	{
		scriptPanel.SetActive(true);
	}

	private void OnClickScriptButton()
	{
		if (currentDialogueIndex == 0)
		{
			StartTypingAnimation("왜 배터리가 없지?");
		}
		else if (currentDialogueIndex == 1)
		{
			StartTypingAnimation("그럼제 어제 나 어떻게 들어왔더라? 뭘 했더라?");
		}
		else if (currentDialogueIndex == 2)
		{
			StartTypingAnimation("기억이 하나도 안나..");
		}
		else if (currentDialogueIndex == 3)
		{
			GameManager.Instance.ToNextSection();
		}

		currentDialogueIndex++;
	}

	void StartTypingAnimation(string script)
	{
		DOTween.To(() => 0, x => scriptText.text = script.Substring(0, x), script.Length, script.Length * 0.1f);
	}

	public void OnClickObject(string objectTextId)
	{
		var clickEvent = GameFlowTable.Instance.GetObjectClickEvent(objectTextId);

		if (clickEvent.EventType == "Explain")
		{
			popupPanel.SetActive(true);
			var imageSprite = ResourceManager.Instance.Load<Sprite>(clickEvent.ImageAsset);
			popupImage.sprite = imageSprite;
			popupText.text = clickEvent.Text;
		}

		if (clickEvent.EventType == "Event")
		{
			if (clickEvent.ObjectTextId == "sun_cellphone")
			{
				Sprite sprite = ResourceManager.Instance.Load<Sprite>("Room_Sun_A_Open");
				roomImage.sprite = sprite;
				cellphonePanel.SetActive(true);
			}
		}

		if (clickEvent.EventType == "GetItem")
		{
			GameManager.Instance.AddItem(clickEvent.ItemTextId);
		}

		if (clickEvent.EventType == "UseItem")
		{
			GameManager.Instance.AddItem(clickEvent.ItemTextId);
		}
	}
}
