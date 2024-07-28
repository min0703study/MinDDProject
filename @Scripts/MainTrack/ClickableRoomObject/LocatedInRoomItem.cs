
using UnityEngine;
using UnityEngine.UI;

public class LocatedInRoomItem : MonoBehaviour
{
	protected UI_MainTrackBase currentTrack;
	[SerializeField] protected string itemTextId;
	[SerializeField] protected GameObject imagePanel;
	
	public string ItemTextId { get => itemTextId; private set { itemTextId = value; }} 
	
	public void Awake()
	{
		var button = GetComponent<Button>();
		currentTrack = GetComponentInParent<UI_MainTrackBase>();
		
		button.onClick.AddListener(OnClickInRoomItem);
		
		InitAwake();
	}
	
	public virtual void InitAwake() 
	{
		bool inLocatedRoom = false;
		if(GameManager.Instance.AllItemStates.TryGetValue(itemTextId, out var state)) 
		{
			inLocatedRoom = state == ItemState.LocatedInRoom;
		} 
		
		if(inLocatedRoom) 
		{
			gameObject.SetActive(true);
			if(imagePanel != null) 
			{
				imagePanel.SetActive(true);
			}
		} else 
		{
			gameObject.SetActive(false);
			if(imagePanel != null) 
			{
				imagePanel.SetActive(false);
			}
		}
	}
	
	public virtual void OnClickInRoomItem() 
	{
		currentTrack.PerformClickInRoomItem(this);
		gameObject.SetActive(false);
		
		if(imagePanel != null) 
		{
			imagePanel.SetActive(false);
		}
	}
}
