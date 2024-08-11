
using System.Collections.Generic;
using UnityEngine;

public class UI_MainTrack_0201 : UI_MainTrackBase
{
	[SerializeField] BlockLock lockC;
	[SerializeField] NumberLock lockB;

	bool isMatch2LockB;
	bool isUnlockedLockA;
	bool isUnlockedLockC;
	
	public override void UpdateMissionState(string objectTextId, RoomObjectEventTriggerType eventState) 
	{
		if(isMatch2LockB == false) 
		{
			if(lockC.MatchBlockCount == 2) 
			{
				Debug.Log("LockC Clear");
				isMatch2LockB = true;
			}
		}
		
		if(isUnlockedLockA == false) 
		{
			if(objectTextId == "entrance_lock_a") 
			{
				Debug.Log("lockA Clear");
				isUnlockedLockA = true;
			}
		}
		
		if(isUnlockedLockC == false) 
		{
			if(lockB.IsUnlocked) 
			{
				Debug.Log("lockB Clear");
				isUnlockedLockC = true;
			}
		}
		
		if(isUnlockedLockA && isMatch2LockB && isUnlockedLockC) 
		{
			GameManager.Instance.ToNextSection();
		}
	}
}
