using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UI_PopupBase : MonoBehaviour
{
	protected bool isInitOver = false;
	public Action OnClosedPopup { get; set; }
	
	private void Awake()
	{
		Init();
		UIManager.Instance.SetCanvas(gameObject, true);
	}
		
	protected virtual void Init()
	{
		if (isInitOver) 
			return;
		
		isInitOver = true;
	}
	
	public virtual void Refresh() {}
	
	public virtual void ClosePopupUI()
	{
		UIManager.Instance.ClosePopupUI(this);
		OnClosedPopup?.Invoke();
	}
	
	public void PopupOpenAnimation(GameObject contentObject)
	{
		contentObject.transform.localScale = new Vector3(0f, 0f, 1f);
		contentObject.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f).SetEase(Ease.InSine);
	}
}
