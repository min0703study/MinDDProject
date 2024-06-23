using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TrackPanel : MonoBehaviour
{
	[Header("Module")]
	[Header("GameUI")]
	[SerializeField] public TextMeshProUGUI missionText;
	[SerializeField] public GameObject inventoryListGO;
	
	[Header("GetItem" )]
	[SerializeField] public GameObject getItemPanel;
	[SerializeField] public Image getItemImage;
	[SerializeField] public TextMeshProUGUI getItemText;
	[SerializeField] public Button getItemNextButton;

}
