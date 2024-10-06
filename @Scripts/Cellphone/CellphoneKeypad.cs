using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellphoneKeypad : MonoBehaviour
{
	public Action<string> OnClickedKeypadButton;
	
	public void PlayShowKeypadAnimation() 
	{
	}
	
	public void PlayHideKeypadAnimation() 
	{
	}
	
	public void OnClickKeypadButton(string key) 
	{
		OnClickedKeypadButton?.Invoke(key);
	}

	public void OnClickBackgroundButton() 
	{
		this.gameObject.SetActive(false);
	}
}
