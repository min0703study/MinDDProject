
using System;

using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CoreLayerBase : MonoBehaviour
{
	protected bool isInitOver = false;
	public Action OnClosedPopup { get; set; }
	
	[SerializeField] private UI_CommonPanel commonPanel;

	#region UI
	protected GameObject scriptPanel => commonPanel.scriptPanel;
	protected TextMeshProUGUI nameText => commonPanel.nameText;
	protected TextMeshProUGUI scriptText => commonPanel.scriptText;
	protected Button scriptNextButton => commonPanel.scriptNextButton;
	protected GameObject popupPanel => commonPanel.popupPanel;

	protected GameObject choicePanel => commonPanel.choicePanel;
	protected GameObject choiceListPanel => commonPanel.choiceListPanel;
	protected GameObject choiceItemHolderPrefab => commonPanel.choiceItemHolderPrefab;
	protected GameObject popupTextPanel => commonPanel.popupTextPanel;
	protected Image popupImage => commonPanel.popupImage;
	protected TextMeshProUGUI popupText => commonPanel.popupText;
	protected Button popupNextButton => commonPanel.popupNextButton;
	protected GameObject characterPanel => commonPanel.characterPanel;
	protected Image characterImage => commonPanel.characterImage;
	protected Image characterMaskImage => commonPanel.characterMaskImage;
	protected GameObject thinkingPanel => commonPanel.thinkingPanel;
	protected GameObject visualSoundEffectPanel => commonPanel.visualSoundEffectPanel;
	protected Image visualSoundEffectImage =>  commonPanel.visualSoundEffectImage;
	protected Button visualSoundEffectNextButton => commonPanel.visualSoundEffectNextButton;
	#endregion
	
	Action popupOnClosed;

	private Tween typingTween;
	private void Awake()
	{
		InitAwake();
	}

	protected virtual void InitAwake()
	{
		if (isInitOver)
			return;

		isInitOver = true;

		if(commonPanel != null) 
		{
			scriptPanel.SetActive(false);
			popupPanel.SetActive(false);
			characterPanel.SetActive(false);
		}
	}

	private void Start()
	{
		UIManager.Instance.SetCanvas(gameObject, false);
	}

	public virtual void Refresh() { }

	public virtual void Clear()
	{

	}

	protected void StartTypingAnimation(string script)
	{
		if (typingTween != null)
		{
			typingTween.Complete();
			typingTween.Kill();
		}
		
		script = script.Replace("<br>", "\n");
		typingTween = DOTween.To(() => 0, x => scriptText.text = script.Substring(0, x), script.Length, script.Length * 0.1f)
			.OnComplete(() => scriptText.text = script);
	}
}
