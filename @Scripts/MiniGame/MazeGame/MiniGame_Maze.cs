using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class MiniGame_Maze : MonoBehaviour
{
	[SerializeField]
	private Image player;
	private Maze maze = new Maze();
	
	[SerializeField]
	private Button rightButton, leftButton, upButton, downButton;
	
	[SerializeField]
	private GridLayoutGroup mazeGrid;
	
	private void Awake() {
		maze.Init();

	}
	
	// Start is called before the first frame update
	void Start()
	{
		player.transform.position = new Vector3(mazeGrid.transform.position.x + (150 * maze.CurrentX),  mazeGrid.transform.position.y + (-150 * maze.CurrentY));
		rightButton.onClick.AddListener(()=>OnClickMoveButton(MoveDirection.Right));
		leftButton.onClick.AddListener(()=>OnClickMoveButton(MoveDirection.Left));
		upButton.onClick.AddListener(()=>OnClickMoveButton(MoveDirection.Up));
		downButton.onClick.AddListener(()=>OnClickMoveButton(MoveDirection.Down));
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	
	void OnClickMoveButton(MoveDirection moveDirection)
	{
		if(maze.IsSafeDirection(moveDirection))
		{
			maze.Move(moveDirection);
			player.transform.position = new Vector3(mazeGrid.transform.position.x + (100 * maze.CurrentX),  mazeGrid.transform.position.y + (-100 * maze.CurrentY));
		}
	}
}

enum MoveDirection 
{
	Up,
	Down,
	Left,
	Right
}

class Maze
{
	static int[,] maze = {
		{ 1, 0, 1, 1, 1, 1, 1 },
		{ 1, 0, 1, 0, 1, 1, 1 },
		{ 1, 0, 1, 1, 0, 1, 1 },
		{ 1, 1, 1, 0, 1, 0, 0 },
		{ 1, 1, 1, 1, 1, 1, 1 }
	};

	static int startX = 0;
	static int startY = 0;
	static int endX = 4;
	static int endY = 6;
	
	public int CurrentX = 0;
	public int CurrentY = 0;

	public void Init() 
	{
		CurrentX = startX;
		CurrentY = startY;
	}

	public bool IsSafe(int row, int col)
	{
		return row >= 0 && row < maze.GetLength(0) && col >= 0 && col < maze.GetLength(1) && maze[row, col] == 1;
	}
	
	public bool IsSafeDirection(MoveDirection moveDirection)
	{
		int x = CurrentX; 
		int y = CurrentY;
		
		switch(moveDirection) 
		{
			case MoveDirection.Up:
				y = CurrentY - 1;
				break;
			case MoveDirection.Down:
				y = CurrentY + 1;
				break;				
			case MoveDirection.Right:
				x = CurrentX + 1;
				break;
			case MoveDirection.Left:
				x = CurrentX - 1;
				break;
		}
		
		//return maze[y,x] == 0;
		return true;
	}
	
	public bool Move(MoveDirection moveDirection)
	{
		switch(moveDirection) 
		{
			case MoveDirection.Up:
				CurrentY -= 1;
				break;
			case MoveDirection.Down:
				CurrentY += 1;
				break;				
			case MoveDirection.Right:
				CurrentX += 1;
				break;
			case MoveDirection.Left:
				CurrentX -= 1;
				break;
		}
		
		return true;
	}

	public int getCurrentIndex()
	{
		return CurrentX + (7 * CurrentY);
	}
}