
using System.Collections.Generic;
using UnityEngine;

public class UI_MainTrack_0202 : UI_MainTrackBase
{
	Dictionary<string, CellphoneApp> appDict = new();
	Dictionary<string, CellphoneScreen> screenDict = new();
	
	private string currentScreenKey;
	private string openAppKey;
	
	protected override void InitAwake()
	{
		base.InitAwake();
		
		CellphoneApp[] cellphoneApps = GetComponentsInChildren<CellphoneApp>();
		if(cellphoneApps != null && cellphoneApps.Length > 0) 
		{
			foreach(var app in cellphoneApps) 
			{
				appDict.Add(app.appKey, app);
				app.gameObject.SetActive(false);
			}
		}
		
		CellphoneScreen[] screens = GetComponentsInChildren<CellphoneScreen>();
		if(screens != null && screens.Length > 0) 
		{
			foreach(var screen in screens) 
			{
				screenDict.Add(screen.ScreenKey, screen);
			}
		}
		
		currentScreenKey = "Lock";
		ChangeScreen(currentScreenKey);
	}
	
	public void ChangeScreen(string screenKey) 
	{
		foreach(var screenPair in screenDict) 
		{
			screenPair.Value.gameObject.SetActive(screenPair.Key == screenKey);
		}
	}
	
	public void OnClickAppButton(string appKey) 
	{
		foreach(var appPair in appDict) 
		{
			appPair.Value.gameObject.SetActive(appPair.Key == appKey);
		}
	}
	
	public void CloseApp() 
	{
		foreach(var appPair in appDict) 
		{
			appPair.Value.gameObject.SetActive(false);
		}
	}
}
