using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public enum Room
{
	SunRoomA,
	LivingRoomA,
	LivingRoomB,
	Kitchen,
}

public class UI_MainTrack_0101 : UI_MainTrackBase
{
	[SerializeField]
	TextMeshProUGUI missionText;

	[SerializeField]
	GameObject popupPanel;

	[SerializeField]
	Image popupImage;

	[SerializeField]
	TextMeshProUGUI popupText;

	[SerializeField]
	Button popupButton;

	Room currentRoom;

	[SerializeField]
	GameObject livingRoomA, livingRoomB, sunRoomA, kitchen;

	private void Awake()
	{
		popupButton.onClick.AddListener(OnClickPopupButton);
		popupPanel.SetActive(false);
	}
	// Start is called before the first frame update
	void Start()
	{
		missionText.text = "문을 열고 밖으로 나가자";
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Move(Room to)
	{
		livingRoomA.SetActive(false);
		livingRoomB.SetActive(false);
		sunRoomA.SetActive(false);
		kitchen.SetActive(false);

		switch (to)
		{
			case Room.LivingRoomA:
				livingRoomA.SetActive(true);
				break;
			case Room.LivingRoomB:
				livingRoomB.SetActive(true);
				break;
			case Room.SunRoomA:
				sunRoomA.SetActive(true);
				break;
			case Room.Kitchen:
				kitchen.SetActive(true);
				break;
		}
	}

	public void OnClickArrowButton(string arrowTextId)
	{
		if (arrowTextId == "to_living_room_a_arrow")
		{
			Move(Room.LivingRoomA);
		}
		else if (arrowTextId == "to_living_room_b_arrow")
		{
			Move(Room.LivingRoomB);
		}
		else if (arrowTextId == "to_kitchen_arrow")
		{
			Move(Room.Kitchen);
		}
	}

	public void OnClickObject(string objectTextId)
	{
		var clickEvent = GameFlowTable.Instance.GetObjectClickEvent(objectTextId);
		if (objectTextId == "sun_room_door")
		{
			Move(Room.LivingRoomA);
		}

		if (clickEvent.EventType == "Text")
		{
			popupPanel.SetActive(true);
			//var imageSprite = ResourceManager.Instance.Load<Sprite>(clickEvent.ImageAsset);
			//popupImage.sprite = imageSprite;
			popupText.text = clickEvent.Text;
		}

		if (clickEvent.EventType == "GetItem")
		{
			GameManager.Instance.AddItem(clickEvent.ItemTextId);
		}

		if (clickEvent.EventType == "UseItem")
		{
			GameManager.Instance.AddItem(clickEvent.ItemTextId);
		}
	}

	public void OnClickPopupButton()
	{
		popupPanel.SetActive(false);
	}

}
