using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MemoApp : CellphoneApp
{
	[SerializeField] TextMeshProUGUI passwordText;
	[SerializeField] CellphoneKeypad cellphoneKeypad;
	[SerializeField] GameObject lockViewGO;
	[SerializeField] GameObject mainViewGO;
	[SerializeField] GameObject detailMemoViewGO;
	[SerializeField] Image detailMemoImage;
	
	bool[] readMemoIndices = new bool[5] { false, false, false, false, false };
	
	bool isLocked = true;
	string password = "20201010";
	int currentMemoIndex = 1;
	
	
	public override void InitAwake()
	{
		cellphoneKeypad.OnClickedKeypadButton += OnClickKeypadButton;
	}

	public override void OpenApp()
	{
		base.OpenApp();
		
		if(isLocked) 
		{
			lockViewGO.SetActive(true);
			mainViewGO.SetActive(false);
		} else 
		{
			lockViewGO.SetActive(false);
			mainViewGO.SetActive(true);
		}
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

		if (passwordText.text == password)
		{
			isLocked = false; 

			lockViewGO.SetActive(false);
			mainViewGO.SetActive(true);
		}
	}
	
	public void OnClickMemoCell(int memoIndex) 
	{
		currentMemoIndex = memoIndex;
		ChangeMemoReadState(currentMemoIndex);
		
		detailMemoViewGO.SetActive(true);
		var imageSprite = ResourceManager.Instance.Load<Sprite>($"Cellphone_Memo_M{memoIndex}");
		detailMemoImage.sprite = imageSprite;
	}
	
	public void OnClickDetailMemoNextButton()
	{
		currentMemoIndex++;
		if (currentMemoIndex > 5)
		{
			currentMemoIndex = 1;
		}
		ChangeMemoReadState(currentMemoIndex);
		
		var imageSprite = ResourceManager.Instance.Load<Sprite>($"Cellphone_Memo_M{currentMemoIndex}");
		detailMemoImage.sprite = imageSprite;
	}

	public void OnClickDetailMemoPrevButton()
	{
		currentMemoIndex--;
		if (currentMemoIndex < 1)
		{
			currentMemoIndex = 5;
		}
		
		ChangeMemoReadState(currentMemoIndex);
		currentTrack.UpdateMissionState();
		var imageSprite = ResourceManager.Instance.Load<Sprite>($"Cellphone_Memo_M{currentMemoIndex}");
		detailMemoImage.sprite = imageSprite;
	}
	
	public void OnClickLockViewBackButton() 
	{
		CloseApp();
	}
	
	public void OnClickDetailMemoViewBackButton() 
	{
		detailMemoViewGO.SetActive(false);
		mainViewGO.SetActive(true);
	}
	
	public void ChangeMemoReadState(int memoIndex) 
	{
		readMemoIndices[memoIndex - 1] = true;
		if(readMemoIndices.Contains(false) == false) 
		{
			currentTrack.UpdateMissionState();
		}
		
	}
}
