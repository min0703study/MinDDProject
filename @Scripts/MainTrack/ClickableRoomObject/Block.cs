using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : ClickableRoomObject
{
	public override void OnClickRoomObjectButton()
	{
		gameObject.SetActive(false);
		GameManager.Instance.AddItem("block_b");
	}
	
}
