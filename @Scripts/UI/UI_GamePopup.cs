using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class UI_GamePopup : UI_PopupBase
{
	[SerializeField]
	private Image popupImage;

	protected override void Init()
	{
		base.Init();
	}

	public void SetInfo(string popupImageAsset)
	{
		var popupSprite = ResourceManager.Instance.Load<Sprite>(popupImageAsset);
		popupImage.sprite = popupSprite;
	}
}
