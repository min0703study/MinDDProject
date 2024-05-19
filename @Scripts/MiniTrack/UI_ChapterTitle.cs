using System.Collections;
using UnityEngine;

public class UI_ChapterTitle : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	
	public void OnPlayChapterTitleAnimation() 
	{
		SoundManager.Instance.Play(SoundManager.SoundType.Effect, "Sound_Keyboard");
	}
	
	
	public void OnCompleteChapterTitleAnimation() 
	{
		SoundManager.Instance.Stop(SoundManager.SoundType.Effect);
		StartCoroutine(CoNextStepDelay());
	}
	
	public IEnumerator CoNextStepDelay() 
	{

		yield return new WaitForSeconds(2f);
		GameManager.Instance.ToNextStep();
	}
}
