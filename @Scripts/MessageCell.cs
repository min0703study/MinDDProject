using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageCell : MonoBehaviour
{
	[SerializeField]
	TextMeshProUGUI messageText;
	
	public void SetMessageText(string text) 
	{
		messageText.text =  text;
	}
}
