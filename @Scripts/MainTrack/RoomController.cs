using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FocusZoneWrapper
{
	public FocusZone FocusZone;
	public GameObject FocusZoneGO;
}

public class RoomController : MonoBehaviour
{
	[SerializeField] RoomType room;
	[SerializeField] List<FocusZoneWrapper> focusZones = new ();
	
	public RoomType Room { get {return room;} }
	public Dictionary<FocusZone, GameObject> FocusZoneDict = new ();
	
	public void Initialize() 
	{
		foreach(var zone in focusZones) 
		{
			FocusZoneDict.Add(zone.FocusZone, zone.FocusZoneGO);
		}
	}
	
	// private void Awake()
	// {

	// }
}
