using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class NumberLock : MonoBehaviour
{
	bool isCollected = false;
	int[] numbers = new int[4];  // 사용자가 조절하는 숫자 배열
	[SerializeField] TextMeshProUGUI[] numberTexts = new TextMeshProUGUI[4];  // 숫자를 표시하는 UI 컴포넌트
	[SerializeField] int[] correctAnswer = new int[4] {1, 2, 3, 4};  // 정답 설정

	[SerializeField] GameObject unlockedGO;
	[SerializeField] GameObject lockedGO;
	private void Awake()
	{
		for (int i = 0; i < numberTexts.Length; i++)
		{
			numberTexts[i].text = numbers[i].ToString();
		}
		
		unlockedGO.SetActive(false);
		lockedGO.SetActive(true);
	}

	public void OnClickRightButton(int index)
	{
		numbers[index] = (numbers[index] + 1) % 10;
		numberTexts[index].text = numbers[index].ToString();
		
		OnClickCheckAnswer();
	}

	public void OnClickLeftButton(int index)
	{
		numbers[index] = (numbers[index] - 1 + 10) % 10;
		numberTexts[index].text = numbers[index].ToString();
		
		OnClickCheckAnswer();
	}

	public void OnClickCheckAnswer()
	{
		if (IsCorrectAnswer())
		{
			isCollected = true;
			
			unlockedGO.SetActive(true);
			lockedGO.SetActive(false);
		}
	}

	private bool IsCorrectAnswer()
	{
		for (int i = 0; i < correctAnswer.Length; i++)
		{
			if (numbers[i] != correctAnswer[i])
				return false;
		}
		return true;
	}
}
