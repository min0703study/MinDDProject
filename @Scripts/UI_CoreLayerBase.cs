using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UI_CoreLayerBase : MonoBehaviour
{
	protected bool isInitOver = false;
	public Action OnClosedPopup { get; set; }

	private void Awake()
	{
		Init();
	}

	protected virtual void Init()
	{
		if (isInitOver)
			return;

		isInitOver = true;
	}

	private void Start()
	{
		UIManager.Instance.SetCanvas(gameObject, false);
	}

	public virtual void Refresh() { }

	public virtual void Clear()
	{

	}

}
