using UnityEngine;
using UnityEngine.UI;

public class MessageApp : CellphoneApp
{
	[SerializeField] Image redDotImage;
	[SerializeField] Image replyBackgroundImage;
	[SerializeField] Image replyDetailBackgroundImage;
		
	[SerializeField] GameObject mainViewGO;
	[SerializeField] GameObject detailMessageViewGO;
	
	bool isReply = false;
	bool isReadMomMessage = false;
	
	public override void OpenApp() 
	{ 
		base.OpenApp();
		mainViewGO.SetActive(true);
		detailMessageViewGO.SetActive(false);
		
		Refresh();
	}
	
	public void OnClickDetailMessageViewBackButton() 
	{
		mainViewGO.SetActive(true);
		detailMessageViewGO.SetActive(false);
	}
	
	public void OnClickDetailMessageCellButton() 
	{
		isReadMomMessage = true;
		Refresh();

		mainViewGO.SetActive(false);
		detailMessageViewGO.SetActive(true);
	}
	
	public void OnClickSendMessageTextboxButton() 
	{
		if(isReply) 
		{
			return;
		}
		
		currentTrack.ShowDialogueBox("엄마한테 문자가 왔었네.<br>답장을 해야겠다. 걱정하실라.", "선", false, OnClickScriptNextButton);
	}
	
	public void Refresh() 
	{
		replyDetailBackgroundImage.gameObject.SetActive(isReply);
		replyBackgroundImage.gameObject.SetActive(isReply);
		
		redDotImage.gameObject.SetActive(!isReadMomMessage);
	}
	
	public void OnClickScriptNextButton () 
	{
		isReply = true;
		Refresh();
		currentTrack.OnClickScriptNextButton -= OnClickScriptNextButton;
	}
}
