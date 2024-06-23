using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLock : MonoBehaviour
{
	[SerializeField] GameObject WhiteBlockGO;
	[SerializeField] GameObject GreenBlockGO;
	
	private void Awake()
	{
	}
	
	public void OnWhiteBlock() 
	{
		WhiteBlockGO.SetActive(true);
	}
	
	public void OnGreenBlock() 
	{
		GreenBlockGO.SetActive(true);
	}
}
