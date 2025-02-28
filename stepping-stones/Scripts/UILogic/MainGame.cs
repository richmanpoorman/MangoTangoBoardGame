using Godot;
using System;
using System.Collections;
using System.Xml.Schema;

public partial class MainGame : Node2D
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	public BoardManager manager;
	private FileSaver saver =  new GameSaver();
	private SceneManager sceneManager;

	private EventBus _eventBus; 
	private BoardManager.GamePhase phase;

	public override void _Ready()
	{
		_eventBus = EventBus.Bus;
		sceneManager = SceneManager.Instance;
		manager = GetNode<BoardManager>("Main/BoardManager");
		CallDeferred(MethodName.DeferredSetupCleanup);

		// Connect(EventBus.SignalName.onPlayerWin, Callable.From(handleWin));
		_eventBus.onPlayerWin += handleWin;
		_eventBus.onPhaseStart += updatePhase;
	}
	private void handleWin()
	{
		GetNode<Control>("GameUI").Visible = false;
		GetNode<Node2D>("Main").Visible = false;
		GetNode<Control>("WinScreen").Visible = true;
	}
	private void DeferredSetupCleanup () {
		configureBoard(sceneManager.board);
	}
	private void configureBoard(SteppingStonesBoard board) {
		// GD.Print("I was deffered :)");
		// GD.Print("1: " + sceneManager.p1Tiles + "tiles, 2: " + sceneManager.p2Tiles);
		manager.setBoard(sceneManager.board);
		manager.setTileCount(Piece.Color.PLAYER_1, sceneManager.p1Tiles);
		manager.setTileCount(Piece.Color.PLAYER_2, sceneManager.p2Tiles);
		manager.setTurn(sceneManager.turn);
		phase = sceneManager.phase;
		manager.setPhase(phase);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void updatePhase(BoardManager.GamePhase newPhase) {
		phase = newPhase;
	}
	public void OnUISaveGame(String path) {
		saver.SaveGame(manager.board(), manager.playerTurn(), 
		manager.playerTileCount(Piece.Color.PLAYER_1),
		manager.playerTileCount(Piece.Color.PLAYER_2),
		phase, path);
	}
	public void OnUIResetGame() {
		Board tmp = manager.board();
		sceneManager.board = new GridSteppingStonesBoard(manager.board().size()[0], manager.board().size()[1]);
		manager.setBoard(sceneManager.board);
		if (!sceneManager.newGame) 
		{
			int tileCount = 0;
			foreach(Tile tile in tmp.tileLayer()) {
				if (tile != null) {
					tileCount++;
				}
			}
			int iniTiles = manager.playerTileCount(Piece.Color.PLAYER_1) + tileCount / 2 - 1;
			sceneManager.p1Tiles = iniTiles;
			sceneManager.p2Tiles = iniTiles;
		}
		manager.setTileCount(Piece.Color.PLAYER_1, sceneManager.p1Tiles);
		manager.setTileCount(Piece.Color.PLAYER_2, sceneManager.p2Tiles);
	}
	public void OnUILoadGame(String path) {
		GD.Print("Game Loaded");
		(SteppingStonesBoard board, Piece.Color turn, int p1Tiles, int p2Tiles, BoardManager.GamePhase phase) 
			= saver.LoadGame(path);
		manager.setBoard(board);	
		manager.setTileCount(Piece.Color.PLAYER_1, p1Tiles);
		manager.setTileCount(Piece.Color.PLAYER_2, p2Tiles);
		manager.setTurn(turn); 
		}
		
}
