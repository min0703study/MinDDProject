using System.Collections;
using System.Collections.Generic;
using TableData;
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
	
	public virtual void InitAwake() {}
	
	public virtual void OnClickInRoomItem() 
	{
		if(ItemTextId == "key_a") 
		{
			currentTrack.ShowPopup("Room_Detail_Living_Cat_Booth_Key", "써니의 숨숨집.<br>하지만 써니는 택배 상자를 더 좋아한다.");
			currentTrack.OnClickPopupNextButton = () =>
			{
				currentTrack.ShowPopup("Room_Detail_Living_Cat_Booth_Key", "엇, 그런데 저기 안에 반짝이는 건 뭐지?");
				currentTrack.OnClickPopupNextButton = () =>
				{
					GameManager.Instance.AddItem(ItemTextId);
					currentTrack.OnClickPopupNextButton = null;
					gameObject.SetActive(false);
				};
			};
		} 
		else if(ItemTextId == "cat_food") 
		{
			currentTrack.ShowPopup("Room_Detail_Kitchen_Under_Drawer_Cat_Food", "써니의 사료. 여기에 있었네.");
			currentTrack.OnClickPopupNextButton = () =>
			{
				GameManager.Instance.AddItem(ItemTextId);
				currentTrack.OnClickPopupNextButton = null;
				gameObject.SetActive(false);
			};
		}
		else 
		{
			currentTrack.PerformClickInRoomItem(this);
			gameObject.SetActive(false);
			
			if(imagePanel != null) 
			{
				imagePanel.SetActive(false);
			}
		}
	}
}
