using UnityEngine;
using UnityEngine.UI;

public class PhoneApp : CellphoneApp
{
    public void OnClickCallButton() 
	{
		currentTrack.ShowDialogueBox("어라, 왜 전화가 안 걸리지. 오류인가.");
	}
	
}
