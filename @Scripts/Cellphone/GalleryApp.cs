using UnityEngine;
using UnityEngine.UI;

public class GalleryApp : CellphoneApp
{
	[SerializeField] GameObject mainViewGO;
	
	[Header("detail photo view")]
	[SerializeField] GameObject detailPhotoViewGO;
	[SerializeField] Image detailPhotoImage;
	[SerializeField] Button detailPhotoBackgroundButton;
	
	public override void InitAwake()
	{
	}
	
	public void OnClickPhotoCell(string photoKey) 
	{
		detailPhotoViewGO.SetActive(true);
		var imageSprite = ResourceManager.Instance.Load<Sprite>($"Cellphone_Gallery_{photoKey}");
		detailPhotoImage.sprite = imageSprite;
		
		if(photoKey == "Photo1") 
		{
			currentTrack.ShowDialogueBox("어릴 때 사진이네. 좀 귀여울지도.", "선", false);
		} 
		else if (photoKey == "Photo2") 
		{
			currentTrack.ShowDialogueBox("하하. 이 사진은 다시 봐도 웃기네.", "선", false);
		}
		else if (photoKey == "Photo3") 
		{
			currentTrack.ShowDialogueBox("윽. 진짜 맛없게 찍어놨잖아.", "선", false);
		}
		else if (photoKey == "Photo4") 
		{
			currentTrack.ShowDialogueBox("이 사진은 왜 찍어둔 거지?", "선", false);
		}
		else if (photoKey == "Photo5") 
		{
			currentTrack.ShowDialogueBox("이 사진은 왜 찍어둔 거지?", "선", false);
		}
		else if (photoKey == "Photo6") 
		{
			currentTrack.ShowDialogueBox("흐음. 뭔가 수상한걸.<br>자세히 살펴봐야겠어.", "선", false);
		}
	}
	
	public void OnClickDetailPhotoBackgroundButton() 
	{
		detailPhotoViewGO.SetActive(false);
	}
	
}
