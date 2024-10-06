using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MessengerApp : CellphoneApp
{
	[SerializeField] TextMeshProUGUI passwordText;
	[SerializeField] CellphoneKeypad cellphoneKeypad;
	[SerializeField] GameObject lockViewGO;
	
	public override void InitAwake()
	{
		cellphoneKeypad.OnClickedKeypadButton += OnClickKeypadButton;
	}

	public override void OpenApp()
	{
		base.OpenApp();
	}
	
	public void OnClickPasswordTextboxButton() 
	{
		cellphoneKeypad.gameObject.SetActive(true);
	}
	
	public void OnClickKeypadButton(string key) 
	{
		if (key == "#")
		{
			if (passwordText.text.Length > 0)
			{
				passwordText.text = passwordText.text.Substring(0, passwordText.text.Length - 1);
			}
		}
		else
		{
			if (passwordText.text.Length < 8) 
			{
				passwordText.text += key;				
			}
		}
	}
	
	
	public void OnClickLockViewBackButton() 
	{
		CloseApp();
	}
}
