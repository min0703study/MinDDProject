using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class RoomWrapper
{
	public RoomType Room;
	public RoomController FocusZoneGO;
}

public class UI_MainTrackBase : UI_CoreLayerBase
{
	[SerializeField] private UI_TrackPanel trackPanel;
	
	protected RoomType CurrentRoom { get; set; } = RoomType.None;
	FocusZone CurrentFocusZone { get; set; }
	
	[SerializeField] List<RoomController> rooms = new ();
	public Dictionary<RoomType, RoomController> RoomDict = new ();
	
	#region UI Components
	protected TextMeshProUGUI missionText => trackPanel.missionText;
	protected GameObject inventoryListGO => trackPanel.inventoryListGO;
	protected GameObject getItemPanel => trackPanel.getItemPanel;
	protected Image getItemImage => trackPanel.getItemImage;
	protected TextMeshProUGUI getItemText => trackPanel.getItemText;
	protected Button getItemNextButton => trackPanel.getItemNextButton;
	#endregion
	
	protected override void InitAwake()
	{
		base.InitAwake();
		
		foreach(var room in rooms) 
		{
			RoomDict.Add(room.Room, room);
		}
		
		foreach(var room in rooms) 
		{
			room.Initialize();
			foreach(var focusZoneGO in room.FocusZoneDict.Values) 
			{
				focusZoneGO.SetActive(false);
			}
			
			room.gameObject.SetActive(false);
		};
	}
	
	public void MoveTo(RoomType moveToRoom, FocusZone moveToFocusZone = FocusZone.None)
	{
		if(moveToFocusZone == FocusZone.None) 
		{
			if(Enum.TryParse($"{moveToRoom}_Full", out FocusZone focusZone))
			{
				moveToFocusZone = focusZone; 
			} else 
			{
				Debug.LogError($"Not Found Focus Zone {$"{moveToRoom}_Full"}");
				return;
			}
		}
		
		if(CurrentRoom != RoomType.None) 
		{
			RoomDict[CurrentRoom].gameObject.SetActive(false);
			RoomDict[CurrentRoom].FocusZoneDict[CurrentFocusZone].SetActive(false);
		}
		
		RoomDict[moveToRoom].gameObject.SetActive(true);
		RoomDict[moveToRoom].FocusZoneDict[moveToFocusZone].SetActive(true);
		
		CurrentRoom = moveToRoom;
		CurrentFocusZone = moveToFocusZone;
	}
	
	public virtual void OnClickRoomObject(ClickableRoomObject gameObject) { }
	public virtual void OnClickMoveArrow(RoomType moveToRoom, FocusZone moveToFocusZone) 
	{
		MoveTo(moveToRoom, moveToFocusZone);
	}
}
