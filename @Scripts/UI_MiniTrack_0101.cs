using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MiniTrack_0101 : UI_MiniTrackBase
{
	[SerializeField]
	Button cellphone;

	[SerializeField]
	Image roomImage;

	// Start is called before the first frame update
	void Start()
	{
		cellphone.onClick.AddListener(OnClickCellphone);
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnClickCellphone()
	{
		Sprite sprite = ResourceManager.Instance.Load<Sprite>("Room_Sun_A_Open");
		roomImage.sprite = sprite;
	}
}
