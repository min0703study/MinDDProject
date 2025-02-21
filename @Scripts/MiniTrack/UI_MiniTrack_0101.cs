
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class UI_MiniTrack_0101 : UI_MainTrackBase
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

	protected override void InitAwake()
	{
		base.InitAwake();

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
			visualSoundEffectPanel.SetActive(false);
			
			var dialog = dialogues[GameManager.Instance.CurrentDetailFlowId];

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

	public override void UpdateMissionState(string objectTextId, RoomObjectEventTriggerType eventState)
	{
		base.UpdateMissionState(objectTextId, eventState);
		
		if (objectTextId == "sun_cellphone")
		{
			Sprite sprite = ResourceManager.Instance.Load<Sprite>("Room_Sun_A_Open");
			roomImage.sprite = sprite;
			cellphonePanel.SetActive(true);
			
			OnClickScriptNextButton = () => 
			{
				GameManager.Instance.ToNextSection();
			};
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
