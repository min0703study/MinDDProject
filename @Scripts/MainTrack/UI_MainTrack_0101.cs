
using UnityEngine;
using UnityEngine.UI;

public class UI_MainTrack_0101 : UI_MainTrackBase
{
	[Header("Room GameObjects")]
	[SerializeField] Image livingRoomAImage;

	public override void UpdateMissionState(string objectTextId, RoomObjectEventTriggerType eventState)
	{
		base.UpdateMissionState(objectTextId, eventState);
		
		if(objectTextId == "sun_room_door") 
		{
			GameManager.Instance.ToNextStep();
		}
		
		if(objectTextId == "living_room_cat_bowl" && eventState == RoomObjectEventTriggerType.UseItem) 
		{
			GameManager.Instance.ToNextStep();
		}
	}

}
