
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UI_MiniTrack_0101 : UI_MiniTrackBase
{
	[Header("Modules")]
	[Header("Cellphone")]

	[SerializeField] GameObject cellphonePanel;

	[SerializeField] Button lockButton;

	[Header("Room")]
	[SerializeField] Image roomImage;

	[SerializeField] TextMeshProUGUI missionText;


	void Start()
	{
		scriptNextButton.onClick.AddListener(OnClickScriptButton);
		popupNextButton.onClick.AddListener(() => popupPanel.SetActive(false));
		lockButton.onClick.AddListener(OnClickLockButton);

		cellphonePanel.SetActive(false);

		Refresh();
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

	private void OnClickLockButton()
	{
		GameManager.Instance.ToNextStep();
		scriptPanel.SetActive(true);
		Refresh();
	}

	private void OnClickScriptButton()
	{
		GameManager.Instance.ToNextStep();
		Refresh();
	}

	public void OnClickRoomObject(string objectTextId)
	{
		var clickEvent = GameFlowTable.Instance.GetObjectClickEvent(objectTextId);

		if (clickEvent.EventType == "Event")
		{
			if (clickEvent.ObjectTextId == "sun_cellphone")
			{
				Sprite sprite = ResourceManager.Instance.Load<Sprite>("Room_Sun_A_Open");
				roomImage.sprite = sprite;
				cellphonePanel.SetActive(true);
			}
		}
		else if (clickEvent.EventType == "Explain")
		{
			popupPanel.SetActive(true);
			var imageSprite = ResourceManager.Instance.Load<Sprite>(clickEvent.ObjectImageAsset);
			popupImage.sprite = imageSprite;
			popupText.text = clickEvent.Text;
		}
	}
}
