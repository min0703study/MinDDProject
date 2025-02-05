
using TableData;
using UnityEngine;

public class UsableItemRoomObject : ClickableRoomObject
{
	[SerializeField] public GameObject UsedItemPanel;
	[SerializeField] protected string useItemTextId;
	[SerializeField] public RoomObjectState changeRoomObjectState;
	bool isUsedItem = false;
	
	public override void OnClickRoomObjectButton() 
	{	
		var itemId = GameManager.Instance.GetSelectedInventoryItemId();
		if (itemId == useItemTextId)
		{
			if(GameManager.Instance.TryUseSelectedItem()) 
			{
				GameManager.Instance.ChangeRoomObjectState(ObjectTextId, RoomObjectState.UsedItem);
				if(UsedItemPanel != null) 
				{
					UsedItemPanel.SetActive(true);	
				}
				
				var roomObjectData = GameFlowTable.Instance.GetRoomObjectEvent(GameManager.Instance.CurrentSection.SectionAsset, objectTextId, RoomObjectEventTriggerType.UseItem);
				if(roomObjectData != null) 
				{
					currentTrack.ShowPopup(roomObjectData.ObjectImageAsset, roomObjectData.Text, ()=> 
					{
						currentTrack.UpdateMissionState(objectTextId, RoomObjectEventTriggerType.UseItem);
					});
				} else 
				{
					currentTrack.UpdateMissionState(objectTextId, RoomObjectEventTriggerType.UseItem);
				}		
			};
		
			
		} else 
		{
			currentTrack.OnClickRoomObject(this);
		}
	}
}
