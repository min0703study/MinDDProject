using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CommonPanel : MonoBehaviour
{
	[Header("Core Layer")]
	[Header("Dialogue GameObjects")]
	public GameObject scriptPanel;
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI scriptText;
	public Button scriptNextButton;

	[Header("Popup GameObjects")]
	public GameObject popupPanel;
	public GameObject popupTextPanel;
	public Image popupImage;
	public TextMeshProUGUI popupText;
	public Button popupNextButton;

	[Header("Character GameObjects")]
	public GameObject characterPanel;
	public Image characterImage;
	public Image characterMaskImage;

}
