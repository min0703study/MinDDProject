using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceItemHolder : MonoBehaviour
{
	private int _index;
	
	Action<int> OnSelectedChoice;
	
	TextMeshProUGUI choiceText;
	Button button;
	
	private void Awake()
	{
		choiceText = GetComponentInChildren<TextMeshProUGUI>();
		button = GetComponent<Button>();
		
		button.onClick.AddListener(() => { OnSelectedChoice?.Invoke(_index);});
	}

	public void Init(string text, int index, Action<int> callback) 
	{
		choiceText.text = text;
		_index = index;
		OnSelectedChoice = callback;
	}
}
