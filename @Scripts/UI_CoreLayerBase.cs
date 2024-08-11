
using System;

using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CoreLayerBase : MonoBehaviour
{
	protected bool isInitOver = false;
	
	[SerializeField] private UI_CommonPanel commonPanel;

	#region UI
	protected GameObject characterPanel => commonPanel.characterPanel;
	protected GameObject twoCharacterPanel => commonPanel.twoCharacterPanel;
	protected Image characterImage => commonPanel.characterImage;
	protected Image characterMaskImage => commonPanel.characterMaskImage;
	
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
	protected GameObject thinkingPanel => commonPanel.thinkingPanel;
	protected GameObject visualSoundEffectPanel => commonPanel.visualSoundEffectPanel;
	protected Image visualSoundEffectImage =>  commonPanel.visualSoundEffectImage;
	protected Button visualSoundEffectNextButton => commonPanel.visualSoundEffectNextButton;
	#endregion
	
	public Action OnClickPopupNextButton;
	public Action OnClickScriptNextButton;
	
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
			AllPanelDisable();
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
		if(script.Contains("<color")) 
		{
			scriptText.text = script;
		} else 
		{
			typingTween = DOTween.To(() => 0, x => scriptText.text = script.Substring(0, x), script.Length, script.Length * 0.1f)
			.OnComplete(() => scriptText.text = script);
		}
	}
	
	protected void ShowCharacter(string imageAsset) 
	{
		if(imageAsset != null && imageAsset != string.Empty) 
		{
			if(imageAsset.Contains(",")) 
			{
				twoCharacterPanel.SetActive(true);
			} else 
			{
				characterPanel.SetActive(true);
				characterImage.gameObject.SetActive(true);
				
				var characterSprite = ResourceManager.Instance.Load<Sprite>(imageAsset);
				characterImage.sprite = characterSprite;
			}
		}
	}
	
	protected void ShowVisualSoundEffectPanel(string imageAsset) 
	{
		visualSoundEffectPanel.SetActive(true);
		visualSoundEffectImage.sprite =  ResourceManager.Instance.Load<Sprite>(imageAsset);
	}
	
	public void ShowDialogueBox(string text, string name = null, bool useTypingAni = true) 
	{
		scriptPanel.SetActive(true);
		if(name != null && name != string.Empty) 
		{
			nameText.text = name;
		}
		
		if(useTypingAni) 
		{
			StartTypingAnimation(text);
		} else 
		{
			scriptText.text = text;
		}
	}
	
	
	public void ShowPopup(string imageAssetKey, string text = null, Action onCloseCallBack = null) 
	{
		popupPanel.SetActive(true);
		
		if(text != null && text != string.Empty) 
		{
			popupTextPanel.SetActive(true);
			popupText.text = text;
		} else 
		{
			popupTextPanel.SetActive(false);
			popupText.text = string.Empty;
		}
		
		if(onCloseCallBack != null) 
		{
			OnClickPopupNextButton = onCloseCallBack;
		}
		
		var popupSprite = ResourceManager.Instance.Load<Sprite>(imageAssetKey);
		popupImage.sprite = popupSprite;
	}
	
	public void OnClickNextButton() 
	{
		
	}
	
	protected void AllPanelDisable() 
	{
		twoCharacterPanel.SetActive(false);
		choicePanel.SetActive(false);
		popupPanel.SetActive(false);
		popupTextPanel.SetActive(false);
		scriptPanel.SetActive(false);
		thinkingPanel.SetActive(false);
		characterImage.gameObject.SetActive(false);
		characterMaskImage.enabled = false;
		visualSoundEffectPanel.SetActive(false);
	}
}
