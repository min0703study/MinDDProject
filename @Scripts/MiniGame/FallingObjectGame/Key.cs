using UnityEngine;

public class Key : MonoBehaviour
{
	private float fallingSpeed = 500.0f;
	private float groundY = 0f;
	private RectTransform rectTransform;
	
	private FallingObjectGame gameController;

	void Start()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	void Update()
	{
		if (rectTransform.anchoredPosition.y > groundY)
		{
			rectTransform.anchoredPosition += new Vector2(0, -fallingSpeed * Time.deltaTime);
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
		gameController.DestroyKey(this);
	}
}