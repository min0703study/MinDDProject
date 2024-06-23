using System;
using TableData;

using UnityEngine;
using UnityEngine.UI;

public class MoveArrow : MonoBehaviour
{
	[SerializeField] RoomType moveToRoom;
	[SerializeField] FocusZone moveToFocusZone;

	public void Awake()
	{
		var button = GetComponent<Button>();
		var track = GetComponentInParent<UI_MainTrackBase>();
		button.onClick.AddListener(() => { track.MoveTo(moveToRoom, moveToFocusZone); });
	}
}
