using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CoreLayerBase : MonoBehaviour
{
	protected bool isInitOver = false;
	public Action OnClosedPopup { get; set; }

	[Header("Core Layer")]
	[Header("Dialogue GameObjects")]
	[SerializeField] protected GameObject scriptPanel;
	[SerializeField] protected TextMeshProUGUI nameText;
	[SerializeField] protected TextMeshProUGUI scriptText;
	[SerializeField] protected Button scriptNextButton;

	[Header("Popup GameObjects")]
	[SerializeField] protected GameObject popupPanel;
	[SerializeField] protected Image popupImage;
	[SerializeField] protected TextMeshProUGUI popupText;
	[SerializeField] protected Button popupNextButton;

	[Header("Character GameObjects")]
	[SerializeField] protected GameObject characterPanel;
	[SerializeField] protected Image characterImage;
	[SerializeField] protected Image characterMaskImage;

	private void Awake()
	{
		Init();
	}

	protected virtual void Init()
	{
		if (isInitOver)
			return;

		isInitOver = true;

		scriptPanel.SetActive(false);
		popupPanel.SetActive(false);
		characterPanel.SetActive(false);
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
		DOTween.To(() => 0, x => scriptText.text = script.Substring(0, x), script.Length, script.Length * 0.1f);
	}
}
