using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BlockItem 
{
	public string BlockItemId;
	public GameObject MatchBlockGO;
}

public class BlockLock : ClickableRoomObject
{
	public List<BlockItem> BlockItems = new();
	public bool[] MathedBlocks { get; set; } = new bool[4];

	public override void InitAwake()
	{
		base.InitAwake();

		foreach(var blockItem in BlockItems) 
		{
			blockItem.MatchBlockGO.SetActive(false);
			blockItem.MatchBlockGO.SetActive(false);
		}
	}
	
	public override void OnClickRoomObjectButton()
	{
		foreach(var blockItem in BlockItems) 
		{
			if(GameManager.Instance.GetSelectedInventoryItemId() == blockItem.BlockItemId) 
			{
				blockItem.MatchBlockGO.SetActive(true);
				GameManager.Instance.UseSelectedItem();
				
				currentTrack.UpdateMissionState(objectTextId, RoomObjectEventTriggerType.UseItem);
				break;
			}
		}
	}
}
