using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_0201_MiniGame : UI_CoreLayerBase
{
	[Header("Module")]
	[Header("GameUI")]
	[SerializeField] TextMeshProUGUI missionText;
	[SerializeField] GameObject inventoryListGO;
	
	[SerializeField] FallingObjectGame fallingObjectGameController;
	
	// Start is called before the first frame update
	void Start()
	{
		SoundManager.Instance.Play(SoundManager.SoundType.Bgm, "Sound_MiniGame0201_BGM", 0.5f);
		inventoryListGO.SetActive(false);
		fallingObjectGameController.OnEndedGame += EndedGame;
	}
	private void OnDestroy()
	{
		fallingObjectGameController.OnEndedGame -= EndedGame;
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}
	
	public override void Clear()
	{
		base.Clear();
		GameManager.Instance.OnChangedStep -= Refresh;
		UIManager.Instance.CloseCoreUI(this);
	}
	
	public void EndedGame() 
	{
		StartCoroutine(CoNextStepDelay());
	}
	
	private void RefreshInventoryList()
	{
		Util.DestroyChilds(inventoryListGO);
		for (int i = 0; i < GameManager.INVENTORY_SIZE; i++)
		{
			var inventoryCell = UIManager.Instance.MakeSubItem<InventoryCell>(inventoryListGO.transform);
			inventoryCell.SetInfo(i, (inventoryCell) =>
			{
				InventorySlot inventorySlot = GameManager.Instance.Inventory[inventoryCell.Index];
				if(inventorySlot.IsBlank) 
				{
					return;
				}
			});

			inventoryCell.Refresh();
		}
	}
	
	public IEnumerator CoNextStepDelay() 
	{
		GameManager.Instance.AddItem("key_b");
		inventoryListGO.SetActive(true);
		RefreshInventoryList();
		SoundManager.Instance.Stop(SoundManager.SoundType.Bgm);
		yield return new WaitForSeconds(4.0f);
		GameManager.Instance.ToNextSection();
	}

}
