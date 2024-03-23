using System;
using TableData;

using UnityEngine;
using UnityEngine.UI;

public class ClickableRoomObject : MonoBehaviour
{
	[SerializeField] string objectTextId;
	public string ObjectTextId { get => objectTextId; private set { objectTextId = value; }} 
	public void Awake()
	{
		var button = GetComponent<Button>();
		var track = GetComponentInParent<UI_MainTrackBase>();
		button.onClick.AddListener(() => { track.OnClickRoomObject(this); });
	}
	
	public void ChangeObjectTextId(string id)
	{
		ObjectTextId = id;
	}
}
