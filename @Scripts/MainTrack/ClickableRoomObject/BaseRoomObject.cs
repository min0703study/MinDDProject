
using UnityEngine;

public class BaseRoomObject : MonoBehaviour
{
	[SerializeField] protected string objectTextId;
	public string ObjectTextId { get => objectTextId; set { objectTextId = value; }} 
	
	protected UI_MainTrackBase currentTrack;
		
	public void Awake()
	{
		currentTrack = GetComponentInParent<UI_MainTrackBase>();
		
		// if(GameManager.Instance.RoomObjectStates.TryGetValue(objectTextId, out RoomObjectState objectState)) 
		// {
		// 	if(objectState != RoomObjectState.LocatedInRoom) 
		// 	{
		// 		gameObject.SetActive(false);
		// 	}
		// }
		
		InitAwake();
	}
	
	public void Update() 
	{
		if(GameManager.Instance.RoomObjectStates.TryGetValue(objectTextId, out RoomObjectState objectState)) 
		{
			if(objectState != RoomObjectState.LocatedInRoom) 
			{
				gameObject.SetActive(false);
			}
		}
	}
	
	public virtual void InitAwake() 
	{
		
	}
}
