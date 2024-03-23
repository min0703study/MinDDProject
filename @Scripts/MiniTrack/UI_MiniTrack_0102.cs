
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class UI_MiniTrack_0102 : UI_MainTrackBase
{
	[Header("GameUI")]
	[SerializeField] TextMeshProUGUI missionText;

	protected override void Init()
	{
		base.Init();

		scriptNextButton.onClick.AddListener(OnClickScriptButton);
		popupNextButton.onClick.AddListener(OnClickPopupNextButton);
	}

	private void Start()
	{
		Bind();
		Refresh();
	}

	private void Bind()
	{
		GameManager.Instance.OnChangedStep += Refresh;
	}

	public override void Refresh()
	{
		var dialogues = GameManager.Instance.CurrentSection.Dialogues;

		if (dialogues != null && dialogues.Count > 0)
		{
			var dialogueIndex = GameManager.Instance.CurrentDialogueIndex;
			var dialog = dialogues[dialogueIndex];

			if (dialog.Type == "mission")
			{
				popupPanel.SetActive(false);
				scriptPanel.SetActive(false);
				characterPanel.SetActive(false);

				missionText.text = dialog.MissionText;
			}
			else if (dialog.Type == "talking_to_myself")
			{
				popupPanel.SetActive(false);
				scriptPanel.SetActive(true);
				characterImage.gameObject.SetActive(false);
				nameText.text = dialog.CharacterKey;

				StartTypingAnimation(dialog.Text);
			}
		}
	}

	private void OnClickScriptButton()
	{
		GameManager.Instance.ToNextStep();
	}

	private void OnClickPopupNextButton()
	{
		popupPanel.SetActive(false);
	}

	public override void OnClickRoomObject(GameObject gameObject, string objectTextId)
	{
		var clickEvent = GameFlowTable.Instance.GetObjectClickEvent(objectTextId);

		if (clickEvent.EventType == "Event")
		{
		}
		else if (clickEvent.EventType == "Explain")
		{
			popupPanel.SetActive(true);
			var imageSprite = ResourceManager.Instance.Load<Sprite>(clickEvent.ObjectImageAsset);
			popupImage.sprite = imageSprite;
			popupText.text = clickEvent.Text;
		}
	}
	
	public override void Clear()
	{
		base.Clear();
		GameManager.Instance.OnChangedStep -= Refresh;
		UIManager.Instance.CloseCoreUI(this);
	}
}
