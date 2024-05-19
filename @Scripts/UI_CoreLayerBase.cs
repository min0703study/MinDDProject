
using System;

using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CoreLayerBase : MonoBehaviour
{
	protected bool isInitOver = false;
	public Action OnClosedPopup { get; set; }
	
	[SerializeField] private UI_CommonPanel common;

	protected GameObject scriptPanel => common.scriptPanel;
	protected TextMeshProUGUI nameText => common.nameText;
	protected TextMeshProUGUI scriptText => common.scriptText;
	protected Button scriptNextButton => common.scriptNextButton;
	protected GameObject popupPanel => common.popupPanel;

	protected GameObject choicePanel => common.choicePanel;
	protected GameObject choiceListPanel => common.choiceListPanel;
	protected GameObject choiceItemHolderPrefab => common.choiceItemHolderPrefab;
	protected GameObject popupTextPanel => common.popupTextPanel;
	protected Image popupImage => common.popupImage;
	protected TextMeshProUGUI popupText => common.popupText;
	protected Button popupNextButton => common.popupNextButton;
	protected GameObject characterPanel => common.characterPanel;
	protected Image characterImage => common.characterImage;
	protected Image characterMaskImage => common.characterMaskImage;
	protected GameObject thinkingPanel => common.thinkingPanel;
	protected GameObject visualSoundEffectPanel => common.visualSoundEffectPanel;
	protected Image visualSoundEffectImage =>  common.visualSoundEffectImage;
	protected Button visualSoundEffectNextButton => common.visualSoundEffectNextButton;

	Action popupOnClosed;

	private Tween typingTween;
	private void Awake()
	{
		Init();
	}

	protected virtual void Init()
	{
		if (isInitOver)
			return;

		isInitOver = true;

		if(common != null) 
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
