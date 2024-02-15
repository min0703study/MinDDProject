using System;
using UnityEngine;

public class GameScene : MonoBehaviour
{	
	// Start is called before the first frame update
	void Start()
	{
		GameManager.Instance.ToNextSection();	
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
