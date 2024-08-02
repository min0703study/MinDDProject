
using TMPro;
using UnityEngine;

public class NumberLock : BaseRoomObject
{
	public bool IsUnlocked { get; set; } = false;
	int[] numbers = new int[4];  // 사용자가 조절하는 숫자 배열
	[SerializeField] TextMeshProUGUI[] numberTexts = new TextMeshProUGUI[4];  // 숫자를 표시하는 UI 컴포넌트
	[SerializeField] int[] correctAnswer = new int[4] {1, 2, 3, 4};  // 정답 설정
	
	public override void InitAwake()
	{
		base.InitAwake();
		
		for (int i = 0; i < numberTexts.Length; i++)
		{
			numberTexts[i].text = numbers[i].ToString();
		}
	}

	public void OnClickNumberKeyButton(int index)
	{
		numbers[index] = (numbers[index] + 1) % 10;
		numberTexts[index].text = numbers[index].ToString();
		
		OnClickCheckAnswer();
	}

	public void OnClickCheckAnswer()
	{
		if (IsCorrectAnswer())
		{
			IsUnlocked = true;
			
			GameManager.Instance.ChangeRoomObjectState(ObjectTextId, RoomObjectState.RemoveRoomObject);
			var roomObjectData = GameFlowTable.Instance.GetRoomObjectEvent(GameManager.Instance.CurrentSection.SectionAsset, objectTextId);
			if(roomObjectData != null) 
			{
				currentTrack.ShowPopup(roomObjectData.ObjectImageAsset, roomObjectData.Text, ()=> 
				{
					currentTrack.UpdateMissionState(objectTextId, RoomObjectEventTriggerType.UseItem);
				});
			} else 
			{
				currentTrack.UpdateMissionState(objectTextId, RoomObjectEventTriggerType.UseItem);
			}
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
