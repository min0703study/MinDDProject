using System.Collections;
using System.Collections.Generic;
using TableData;
using UnityEngine;

public class CellphoneScreen : MonoBehaviour
{
	public string ScreenKey;	
	protected UI_MainTrack_0202 currentTrack;
	
	public void Awake()
	{
		currentTrack = GetComponentInParent<UI_MainTrack_0202>();
	}
	
	public void OnClickAppButton(string appKey) 
	{
		currentTrack.OnClickAppButton(appKey);
	}
	
	public void ChangeScreen(string screenKey) 
	{
		currentTrack.ChangeScreen(screenKey);
	}
}
