using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item", order = 0)]
public class ItemSO : ScriptableObject
{
	[SerializeField]
	public int ItemId;
	
	[SerializeField]
	public string Name;
		
	[SerializeField]
	public Sprite itemSprite;
	
	[SerializeField]
	public string Desc;
}
