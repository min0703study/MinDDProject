using System.Collections;
using System.Collections.Generic;
using TableData;
using UnityEngine;

public class TableLoader : BaseManager<TableLoader>
{
	protected override void initAwake()
	{
		base.initAwake();
	}

	public void Load()
	{
		GameFlowTable.Instance.Load("Table_GameFlow",
			"Table_GameFlowDetail",
			"Table_ObjectClickEvent",
			"Table_Item",
			"Table_RoomObject");
	}

}
