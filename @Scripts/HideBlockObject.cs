using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBlockObject : MonoBehaviour
{
	[SerializeField] GameObject hideItemGO;
	[SerializeField] GameObject getItemGO;
	
	private void Awake()
	{
		hideItemGO.SetActive(true);
		getItemGO.SetActive(false);
	}
	public void OnClickBlockItem() 
	{
		hideItemGO.SetActive(false);
		getItemGO.SetActive(true);
	}
}
