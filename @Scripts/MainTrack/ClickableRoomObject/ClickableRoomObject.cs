using System;
using TableData;

using UnityEngine;
using UnityEngine.UI;

public class ClickableRoomObject : MonoBehaviour
{
	protected UI_MainTrackBase currentTrack;
	[SerializeField] protected string objectTextId;
	
	public string ObjectTextId { get => objectTextId; private set { objectTextId = value; }} 
	[SerializeField] public GameObject UsedItemPanel;
	
	public void Awake()
	{
		var button = GetComponent<Button>();
		currentTrack = GetComponentInParent<UI_MainTrackBase>();
		
		button.onClick.AddListener(OnClickRoomObjectButton);
		InitAwake();
	}
	
	public virtual void InitAwake() {}
	
	public virtual void OnClickRoomObjectButton() 
	{
		currentTrack.OnClickRoomObject(this);
	}
	
	public void ChangeObjectTextId(string id)
	{
		ObjectTextId = id;
	}
	
	public void ChangeImageForUsedItem()
	{
		if(UsedItemPanel != null) 
		{
			UsedItemPanel.SetActive(true);
		}
	}
}
