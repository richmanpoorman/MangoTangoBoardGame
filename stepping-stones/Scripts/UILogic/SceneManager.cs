using Godot;
using System;

public partial class SceneManager : Node
{
	public SteppingStonesBoard board {get; set;}
	public Piece.Color turn {get; set;}
	public int p1Tiles {get; set;}
	public int p2Tiles {get; set;}
	public BoardManager.GamePhase phase {get; set;}

	public bool newGame {get; set;}
	[Export]
	private String _mainSceneFile = "res://Scenes/Main.tscn";
	private String _titleSceneFile = "res://Scenes/titlescreen.tscn";
	public static SceneManager Instance {get; private set;}
	// public int width {set; get;}
	// public int length {set; get;}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		board = new GridSteppingStonesBoard(5, 7);
	}
	public void goToMainBoard (Tuple<SteppingStonesBoard, Piece.Color, int, 
									int, BoardManager.GamePhase> tuple) {
		(board, turn, p1Tiles, p2Tiles, phase) = tuple;
		GetTree().ChangeSceneToFile(_mainSceneFile);
	}


	public void goToMainBoard(SteppingStonesBoard board) {
		this.board = board;
		GetTree().ChangeSceneToFile(_mainSceneFile);
	}

	public void goToMainBoard(SteppingStonesBoard board, int tiles) {
		this.board = board;
		this.p1Tiles = tiles;
		this.p2Tiles = tiles;
		turn = Piece.Color.PLAYER_1;
		GetTree().ChangeSceneToFile(_mainSceneFile);
	}

	
	public void goToTitleScreen() {
		GetTree().ChangeSceneToFile(_titleSceneFile);
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
