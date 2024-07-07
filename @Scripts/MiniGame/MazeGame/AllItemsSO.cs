using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items", order = 0)]
public class AllItemsSO : ScriptableObject
{
	[Serializable]
	public class Item
	{
		public string ItemTextId;
		public string TextObjectId;
	}

	public List<Item> Items;
}
