using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallingObjectGame : MonoBehaviour
{
	[Header("GameObjects")]
	[SerializeField] private GameObject obstaclePrefab;
	[SerializeField] private GameObject keyPrefab;
	[SerializeField] private PlayerController player;
	
	[Header("UI")]
	[SerializeField] private Button moveLeftButton;
	[SerializeField] private Button moveRightButton;
	[SerializeField] private Slider gameTimeSlider;
	[SerializeField] private GameObject clearTextPanel;
	
	[Header("Game Play Values")]
	[SerializeField] private float clearGameTime;
	[SerializeField] private float obstacleSpawnInterval = 1.0f;
	[SerializeField] private float playerMoveSpeed = 40f;
	[SerializeField] private float fallingSpeed = 500f;
	[SerializeField] private float groundY = 0;

	private float obstacleSpawnTimer;
	private float elapsedGameTimer;
	
	private List<Obstacle> obstacles = new List<Obstacle>();
	
	private bool isGameWon = false;
	
	private void Awake()
	{
		moveLeftButton.onClick.AddListener(()=>{ OnClickMoveButton(true); });
		moveRightButton.onClick.AddListener(()=>{ OnClickMoveButton(false); });
	}
	
	private void Start()
	{
		player.Initialize(this);
	}

	void Update()
	{
		if(isGameWon == false) 
		{
			obstacleSpawnTimer += Time.deltaTime;
			if (obstacleSpawnTimer >= obstacleSpawnInterval)
			{
				SpawnObstacle();
				obstacleSpawnTimer = 0;
			}
			
			elapsedGameTimer += Time.deltaTime;
			 UpdateClearTimeSlider();
			if (elapsedGameTimer >= clearGameTime)
			{
				WinGame();
				elapsedGameTimer = 0;
			}
		}
	}
	
	void UpdateClearTimeSlider() 
	{
		gameTimeSlider.value = elapsedGameTimer / clearGameTime;
	}

	void SpawnObstacle()
	{
		float xPosition = Random.Range(-Screen.width / 2, Screen.width / 2);
		Vector3 spawnPosition = new Vector3(xPosition, Screen.height / 2, 0);
		GameObject newObstacle = Instantiate(obstaclePrefab, transform);
		
		newObstacle.GetComponent<RectTransform>().anchoredPosition = spawnPosition;
		newObstacle.GetComponent<Obstacle>().Initialize(this, fallingSpeed, groundY);
		obstacles.Add(newObstacle.GetComponent<Obstacle>());
	}
	
	void SpawnKey()
	{
		Vector3 spawnPosition = new Vector3(0, Screen.height / 2, 0);
		GameObject newObstacle = Instantiate(keyPrefab, transform);
		
		newObstacle.GetComponent<RectTransform>().anchoredPosition = spawnPosition;
		newObstacle.GetComponent<Key>().Initialize(this, 400, groundY);
	}
	
	public void DestroyObstacle(Obstacle obstacle)
	{
		obstacle.gameObject.SetActive(false);
		obstacles.Remove(obstacle);
		Destroy(obstacle.gameObject);
	}
	
	public void DestroyKey(Key key)
	{
		Destroy(key.gameObject);
	}
	
	
	void WinGame()
	{
		isGameWon = true;
		foreach(var obstacle in obstacles) 
		{
			obstacle.gameObject.SetActive(false);
			Destroy(obstacle.gameObject);
		}
		
		obstacles.Clear();
		clearTextPanel.SetActive(true);
		
		SpawnKey();
	}
	
	void OnClickMoveButton(bool isLeft) 
	{
		if(isLeft) 
		{
			player.Move(-playerMoveSpeed);
		} else 
		{
			player.Move(playerMoveSpeed);
		}
	}
	
	public void HandleObstacleCollision() 
	{
		elapsedGameTimer -= 3f;
	}
	
	public void HandleKeyCollision() 
	{
		elapsedGameTimer -= 3f;
	}
}
