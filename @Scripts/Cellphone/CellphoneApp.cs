
using UnityEngine;
using UnityEngine.UI;

public class CellphoneApp : MonoBehaviour
{
	protected UI_MainTrack_0202 currentTrack;
	[SerializeField] Button backButton;
	
	[SerializeField] public string appKey;
	
	public void Awake()
	{
		currentTrack = GetComponentInParent<UI_MainTrack_0202>();
		backButton.onClick.AddListener(()=> 
		{
			currentTrack.CloseApp();
		});
		
		InitAwake();
	}
	
	public virtual void InitAwake() 
	{

	}
}
