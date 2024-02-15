using System.Collections;
using System.Collections.Generic;
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
			"Table_Dialogue",
			"Table_MiniTrack",
			"Table_MainTrack",
			"Table_ObjectClickEvent");
	}

}
