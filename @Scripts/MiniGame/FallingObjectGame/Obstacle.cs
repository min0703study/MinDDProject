using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
	private float fallingSpeed = 0f;
	private float groundY = 0f;
	private RectTransform rectTransform;
	
	public FallingObjectGame gameController;
	
	public bool isStopFalling = false;

	void Start()
	{
		rectTransform = GetComponent<RectTransform>();
	}


	void Update()
	{
		if(isStopFalling == false) 
		{
			rectTransform.anchoredPosition += new Vector2(0, -fallingSpeed * Time.deltaTime);
			if (rectTransform.anchoredPosition.y < groundY)
			{
				gameController.DestroyObstacle(this);
			}
		}
	}
	
	public void Initialize(FallingObjectGame gameController, float fallingSpeed, float groundY) 
	{
		this.gameController = gameController;
		this.fallingSpeed = fallingSpeed;
		this.groundY = groundY;		
	}
		
	public void HandleCollision()
	{
		gameController.DestroyObstacle(this);
		if(isStopFalling == false) 
		{
			gameController.HandleObstacleCollision();	
		}
	}

	internal void StopFalling()
	{
		isStopFalling = true;
	}
}