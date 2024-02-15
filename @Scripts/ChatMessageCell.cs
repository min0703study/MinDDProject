using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatMessageCell : MonoBehaviour
{
	[SerializeField]
	GameObject messageListGO;
	
	[SerializeField]
	GameObject chatMessageCellPrefab;
	// Start is called before the first frame update
	
	public bool IsMessageAllShowing = false;

	private void Awake() {
		Util.DestroyChilds(messageListGO);	
	}
	
	public void AddMessageText(string text) 
	{
		var messageCellGO = Instantiate(chatMessageCellPrefab);
		var messageCell = messageCellGO.GetComponent<MessageCell>();
		messageCell.SetMessageText(text);
		messageCellGO.SetActive(false);
		
		messageCellGO.transform.SetParent(messageListGO.transform, false);
	}
	
	public void ShowMessage() 
	{
		IsMessageAllShowing = true;	
		for(int i = 0; i < messageListGO.transform.childCount; i++)
		{
			if(messageListGO.transform.GetChild(i).gameObject.activeSelf == false)
			{
				messageListGO.transform.GetChild(i).gameObject.SetActive(true);
				// do what you need with it
				IsMessageAllShowing = false;
				break;
			}

		}
	}
}
