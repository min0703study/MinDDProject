
using UnityEngine;
using UnityEngine.UI;

public class ClickableRoomObject : BaseRoomObject
{	
	public override void InitAwake()
	{
		base.InitAwake();
		
		var button = GetComponent<Button>();
		button.onClick.AddListener(OnClickRoomObjectButton);
	}
	
	public virtual void OnClickRoomObjectButton() 
	{
		currentTrack.OnClickRoomObject(this);
	}
	
	public void ChangeObjectTextId(string id)
	{
		ObjectTextId = id;
	}
}
