using UnityEngine;

public class KeyLock : ClickableRoomObject
{
	[SerializeField] private string keyItemTextId;
	
	public override void OnClickRoomObjectButton()
	{
		if(GameManager.Instance.GetSelectedInventoryItemId() == keyItemTextId) 
		{
			GameManager.Instance.UseItem(GameManager.Instance.SelectedInventoryIndex);
			
			currentTrack.UpdateMissionState();
		}
	}
}
