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
		
		if(photoKey == "Photo3") 
		{
			currentTrack.ShowDialogueBox("윽. 진짜 맛없게 찍어놨잖아.");
		} 
		else if (photoKey == "Photo4") 
		{
			currentTrack.ShowDialogueBox("이 사진은 왜 찍어둔 거지?");
		}
		else if (photoKey == "Photo5") 
		{
			currentTrack.ShowDialogueBox("이 사진은 왜 찍어둔 거지?");
		}
	}
	
	public void OnClickDetailPhotoBackgroundButton() 
	{
		detailPhotoViewGO.SetActive(false);
	}
	
}
