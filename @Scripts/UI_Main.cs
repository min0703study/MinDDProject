using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Main : MonoBehaviour
{
	[SerializeField]
	GameObject chatListGO;
	
	[SerializeField]
	GameObject chatMessagePrefab, chatIllustPrefab, chatIllustPrefab2;
	[SerializeField]
	GameObject handllustPrefab;
	
	[SerializeField]
	GameObject UICellPhoneTrack;
	
	Queue<GameObject> storyQueue = new Queue<GameObject>();
	
	[SerializeField]
	Button nextButton;
	
	// Start is called before the first frame update
	private void Awake() {
		Util.DestroyChilds(chatListGO);
		nextButton.onClick.AddListener(OnClickNextButton);
	}
	
	void Start()
	{
		var illust = Instantiate(chatIllustPrefab2);	
		//storyQueue.Enqueue(illust);
		//illust.transform.SetParent(chatListGO.transform, false);	
		
		
		illust = Instantiate(chatIllustPrefab);	
		//storyQueue.Enqueue(illust);
		
		var sendMessage = Instantiate(chatMessagePrefab);
		var chatMessage = sendMessage.GetComponent<ChatMessageCell>();
		chatMessage.AddMessageText("윽 머리가 너무 아픈데.");
		chatMessage.AddMessageText("아니 나 왜 문 앞에서 잠들어 있는 거야?");
		chatMessage.AddMessageText("역시 형이 약을...신고해야겠어..");
		chatMessage.AddMessageText("자물쇠도 다 풀었잖아?");
		
		//storyQueue.Enqueue(sendMessage);
		
		illust = Instantiate(handllustPrefab);	
		storyQueue.Enqueue(illust);
		
		sendMessage = Instantiate(chatMessagePrefab);
		chatMessage = sendMessage.GetComponent<ChatMessageCell>();
		chatMessage.AddMessageText("무슨 일이지? 전화가 안 눌리잖아.");
		chatMessage.AddMessageText("문자도 그렇고");
		chatMessage.AddMessageText("통신에 문제가 있는 건가?");
		chatMessage.AddMessageText("하지만 그럼 이 메시지는 뭐지?");
		chatMessage.AddMessageText("메신저를 확인해보자.");
		
		storyQueue.Enqueue(sendMessage);
		//sendMessage.transform.SetParent(chatListGO.transform, false);	
		

		//illust.transform.SetParent(chatListGO.transform, false);	
		
		
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	
	public void OnClickNextButton()
	{
		if(storyQueue.Count == 0) 
		{
			UICellPhoneTrack.SetActive(true);
			gameObject.SetActive(false);
		}
		
		var message = storyQueue.Peek();
		
		var chatMessageCell = message.GetComponent<ChatMessageCell>();
		if(chatMessageCell != null) 
		{
			if(message.transform.parent == null)
			{
				message.transform.SetParent(chatListGO.transform, false);
			}
			chatMessageCell.ShowMessage();
			
			if(chatMessageCell.IsMessageAllShowing) 
			{
				storyQueue.Dequeue();
			}
		} else 
		{
			message.transform.SetParent(chatListGO.transform, false);
			storyQueue.Dequeue();
		}
		

	}
}
