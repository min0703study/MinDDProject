using System.Collections;
using System.Collections.Generic;
using TableData;
using UnityEngine;

public class TableLoader : BaseManager<TableLoader>
{
	protected override void init()
	{
		base.init();
	}

	public void Load()
	{
		GameFlowTable.Instance.Load("Table_GameFlow",
			"Table_GameFlowDetail",
			"Table_ObjectClickEvent",
			"Table_Item");
	}

}
