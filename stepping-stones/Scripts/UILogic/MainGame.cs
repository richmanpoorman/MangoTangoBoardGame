using Godot;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Xml.Schema;

public partial class MainGame : Node2D
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	public BoardManager manager {get; set;}
	private FileSaver saver =  new GameSaver();
	private SceneManager sceneManager;

	private EventBus _eventBus; 
	public GamePhase phase;

	[Export]
	private GameUi gameUi;

	private bool _newGame;
	private int _p1Tiles;
	private int _p2Tiles;

	private SteppingStonesBoard _board;

	public override void _Ready()
	{
		GD.Print("Faith rewarded");
		_eventBus = EventBus.Bus;
		sceneManager = SceneManager.Instance;
		GD.Print($"SceneManager p1_tiles: {sceneManager.p1Tiles}");
		manager = GetNode<BoardManager>("Main/BoardManager");
		CallDeferred(MethodName.SetGameToSceneManager);

		// Connect(EventBus.SignalName.onPlayerWin, Callable.From(handleWin));
		_eventBus.onPlayerWin  += handleWin;
		_eventBus.onPhaseStart += updatePhase;
		_eventBus.onGameReset  += OnResetGame;
		_eventBus.onSetGameToSceneManagerRequest += SetGameToSceneManager;
		_p1Tiles = sceneManager.p1Tiles;
		_p2Tiles = sceneManager.p2Tiles;
		_newGame = sceneManager.newGame;
		_board = sceneManager.board;
	}

    public override void _ExitTree()
    {
        _eventBus.onPlayerWin  -= handleWin;
		_eventBus.onPhaseStart -= updatePhase;
		_eventBus.onGameReset  -= OnResetGame; 
    }

	private void handleWin()
	{
		GetNode<Control>("GameUI").Visible = false;
		gameUi.hideAll();
		//GetNode<Node2D>("Main").Visible = false;
		GetNode<Control>("WinScreen").Visible = true;
	}
	private void SetGameToSceneManager() {
		GD.Print("setting game to manager game");
		configureBoard(SceneManager.Instance.board);
	}
	private void configureBoard(SteppingStonesBoard board) {
		// GD.Print("I was deffered :)");
		// GD.Print("1: " + sceneManager.p1Tiles + "tiles, 2: " + sceneManager.p2Tiles);
		// GD.Print("configuring board");
		manager.setBoard(_board);
		manager.setTileCount(PlayerColor.PLAYER_1, sceneManager.p1Tiles);
		manager.setTileCount(PlayerColor.PLAYER_2, sceneManager.p2Tiles);
		manager.setTurn(sceneManager.turn);
		phase = sceneManager.phase;
		// GD.Print($"config phase: {phase}");
		manager.setPhase(phase);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void updatePhase(GamePhase newPhase) {
		phase = newPhase;
	}
	public void OnUISaveGame(String path) {
		saver.SaveGame(manager.board(), manager.playerTurn(), 
		manager.playerTileCount(PlayerColor.PLAYER_1),
		manager.playerTileCount(PlayerColor.PLAYER_2),
		phase, path);
	}
	public void OnResetGame() {
		GD.Print("resetting");
		gameUi.showAll();
		
		Board tmp = manager.board();
		if (!_newGame) 
		{
			int tileCount = 0;
			foreach(Tile tile in tmp.tileLayer()) {
				if (tile != null) {
					tileCount++;
				}
			}
			GD.Print("p1tiles: " +  _p1Tiles +", p2tiles:" + _p2Tiles + ", tilecount: " + tileCount);
			int iniTiles = ((Math.Max(_p1Tiles, _p2Tiles) + tileCount) / 2) - 1;
			_p1Tiles = iniTiles;
			_p2Tiles = iniTiles;
			_newGame = true;
		}
		_board = new GridSteppingStonesBoard(manager.board().size()[0], manager.board().size()[1]);
		manager.setBoard(_board);

		manager.setTileCount(PlayerColor.PLAYER_1, _p1Tiles);
		manager.setTileCount(PlayerColor.PLAYER_2, _p2Tiles);
		gameUi.updateBlueTiles(_p1Tiles);
		gameUi.updateRedTiles(_p2Tiles);
	}
	public void OnUILoadGame(String path) {
		GD.Print("Game Loaded");
		(SteppingStonesBoard board, PlayerColor turn, _p1Tiles, _p2Tiles, GamePhase phase) 
			= saver.LoadGame(path);
		manager.setBoard(board);	
		manager.setTileCount(PlayerColor.PLAYER_1, _p1Tiles);
		manager.setTileCount(PlayerColor.PLAYER_2, _p2Tiles);
		manager.setTurn(turn);
	}

	
		
}
