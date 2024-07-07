using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item", order = 0)]
public class ItemSO : ScriptableObject
{
	public string ItemTextId;
	public string ItemName;
	public Sprite ItemImage;
	public ItemState ItemState;
}
