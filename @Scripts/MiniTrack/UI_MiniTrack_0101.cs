
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class UI_MiniTrack_0101 : UI_MiniTrackBase
{
	[Header("Modules")]
	[Header("Cellphone")]
	[SerializeField] GameObject cellphonePanel;
	[SerializeField] GameObject lockScreen;
	[SerializeField] GameObject noBatteryScreen;
	[SerializeField] GameObject turnOffScreen;
	[SerializeField] GameObject mainScreen;
	[SerializeField] Button lockButton;

	[Header("Room")]
	[SerializeField] Image roomImage;

	[Header("GameUI")]
	[SerializeField] TextMeshProUGUI missionText;

	void Awake()
	{
		scriptNextButton.onClick.AddListener(OnClickScriptButton);
		popupNextButton.onClick.AddListener(OnClickPopupNextButton);
		lockButton.onClick.AddListener(OnClickLockButton);

		cellphonePanel.SetActive(false);

		Bind();
	}

	private void Start()
	{
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

	private void OnClickLockButton()
	{
		lockScreen.SetActive(false);
		StartCoroutine(StartCellphoneAnimation());
	}

	private void OnClickScriptButton()
	{
		GameManager.Instance.ToNextStep();
	}

	private void OnClickPopupNextButton()
	{
		popupPanel.SetActive(false);
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

	public IEnumerator StartCellphoneAnimation()
	{
		mainScreen.SetActive(true);
		yield return new WaitForSeconds(1.5f);
		mainScreen.SetActive(false);
		noBatteryScreen.SetActive(true);
		yield return new WaitForSeconds(1.0f);
		noBatteryScreen.SetActive(false);
		turnOffScreen.SetActive(true);
		yield return new WaitForSeconds(1.0f);

		GameManager.Instance.ToNextStep();
	}

	public override void Clear()
	{
		base.Clear();
		GameManager.Instance.OnChangedStep -= Refresh;
		UIManager.Instance.CloseCoreUI(this);
	}
}
