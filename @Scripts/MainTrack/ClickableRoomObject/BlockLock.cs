using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLock : ClickableRoomObject
{
	public bool[] MathedBlocks { get; set; } = new bool[4];
	
 	[SerializeField] GameObject BlockAGO;
	[SerializeField] GameObject BlockBGO;

	public override void InitAwake()
	{
		base.InitAwake();
		
		BlockBGO.SetActive(false);
		BlockAGO.SetActive(false);
	}
	
	public override void OnClickRoomObjectButton()
	{
		if(GameManager.Instance.GetSelectedInventoryItemId() == "block_a") 
		{
			BlockAGO.SetActive(true);
			GameManager.Instance.UseItem(GameManager.Instance.SelectedInventoryIndex);
			
			currentTrack.UpdateMissionState();
		} else if (GameManager.Instance.GetSelectedInventoryItemId() == "block_b") 
		{
			BlockBGO.SetActive(true);
			GameManager.Instance.UseItem(GameManager.Instance.SelectedInventoryIndex);
			
			currentTrack.UpdateMissionState();
		}
	}
	
}
