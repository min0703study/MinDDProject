using UnityEngine;

public class KeyLock : ClickableRoomObject
{
	[SerializeField] private string keyItemTextId;

	public bool IsUnlocked { get; internal set; }

	public override void OnClickRoomObjectButton()
	{
		if(GameManager.Instance.GetSelectedInventoryItemId() == keyItemTextId) 
		{
			IsUnlocked = true;
			GameManager.Instance.UseItem(GameManager.Instance.SelectedInventoryIndex);
			
			currentTrack.UpdateMissionState(objectTextId, RoomObjectEventTriggerType.UseItem);
		}
	}
}
