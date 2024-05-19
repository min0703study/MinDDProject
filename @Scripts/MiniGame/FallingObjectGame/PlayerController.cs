using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	private RectTransform rectTransform;
	private FallingObjectGame gameController;

	[SerializeField] private GameObject normalPanel;
	[SerializeField] private GameObject winPanel;

	void Start()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	public void Initialize(FallingObjectGame gameController)
	{
		this.gameController = gameController;
	}

	public void Move(float moveTo)
	{
		rectTransform.anchoredPosition += new Vector2(moveTo, 0);

		// 화면 밖으로 나가지 않도록 제한
		float clampedX = Mathf.Clamp(rectTransform.anchoredPosition.x, -Screen.width / 2, Screen.width / 2);
		rectTransform.anchoredPosition = new Vector2(clampedX, rectTransform.anchoredPosition.y);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Obstacle"))
		{
			collision.gameObject.GetComponent<Obstacle>().HandleCollision();
		}
		else if (collision.CompareTag("Key"))
		{
			collision.gameObject.GetComponent<Key>().HandleCollision();
			normalPanel.SetActive(false);
			winPanel.SetActive(true);
		}
	}
	
	public void OnCompleteWinAnimation() 
	{
		normalPanel.SetActive(true);
		winPanel.SetActive(false);
		
		gameController.HandleKeyCollision();
	}

}