using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLock : ClickableRoomObject
{
	public bool[] MathedBlocks { get; set; } = new bool[4];
	
 	[SerializeField] GameObject MatchBlockWGO;
	[SerializeField] GameObject MatchBlockGGO;

	public override void InitAwake()
	{
		base.InitAwake();
		
		MatchBlockGGO.SetActive(false);
		MatchBlockWGO.SetActive(false);
	}
	
	public override void OnClickRoomObjectButton()
	{
		if(GameManager.Instance.GetSelectedInventoryItemId() == "block_w") 
		{
			MatchBlockWGO.SetActive(true);
			GameManager.Instance.UseItem(GameManager.Instance.SelectedInventoryIndex);
			
			currentTrack.UpdateMissionState();
		} else if (GameManager.Instance.GetSelectedInventoryItemId() == "block_g") 
		{
			MatchBlockGGO.SetActive(true);
			GameManager.Instance.UseItem(GameManager.Instance.SelectedInventoryIndex);
			
			currentTrack.UpdateMissionState();
		}
	}
	
}
