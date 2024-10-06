
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainTrack_0202 : UI_MainTrackBase
{
	Dictionary<string, CellphoneApp> appDict = new();
	Dictionary<string, CellphoneScreen> screenDict = new();
	
	private string currentScreenKey;
	private string openAppKey;
	
	[SerializeField] Button exitButton;
	
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
		if(appDict.TryGetValue(appKey, out var app)) 
		{
			openAppKey = appKey;
			app.OpenApp();
		};
	}
	
	public void CloseApp() 
	{
		if(appDict.TryGetValue(openAppKey, out var app)) 
		{
			openAppKey = string.Empty;
			app.CloseApp();
		};
	}
	
	public override void UpdateMissionState() 
	{
		exitButton.gameObject.SetActive(true);
	}
	
	public void OnClickExitButton() 
	{
		GameManager.Instance.ToNextSection();
	}
}
