using Godot;
using System;

public partial class GameUi : Control
{
	// Called when the node enters the scene tree for the first time.
	
	[Signal]
	public delegate void ResetGameEventHandler();
	[Signal]
	public delegate void SaveGameEventHandler(String path);
	[Signal]
	public delegate void LoadGameEventHandler(String path);

	[Export]
	public FileDialog saveBox;
	
	[Export]
	public FileDialog loadBox;
	
	[Export]
	public RichTextLabel turnLabel;

	[Export]
	public RichTextLabel redTiles;

	[Export]
	public RichTextLabel blueTiles;

	private Piece.Color currentPlayer;

	private BoardManager manager;
	private int p1Tiles, p2Tiles;
	private EventBus _eventBus; 
	private SceneManager sceneManager;

	private void OnResetButtonPressed() 
	{
		EmitSignal(SignalName.ResetGame);
	}
	private void OnSaveButtonPressed() 
	{
		saveBox.Popup();
	}

	private void OnLoadButtonPressed()
	{
		loadBox.Popup();
	}
	private void OnSaveDialogFileSelected(String path) 
	{
		EmitSignal(SignalName.SaveGame, path);
	}
	private void OnLoadDialogFileSelected(String path) 
	{
		EmitSignal(SignalName.LoadGame, path);
	}
	public override void _Ready()
	{
		_eventBus = EventBus.Bus;
		sceneManager = SceneManager.Instance;
		manager = GetParent().GetNode<BoardManager>("Main/BoardManager");
		currentPlayer = manager.playerTurn();
		p1Tiles = sceneManager.p1Tiles;
		p2Tiles = sceneManager.p2Tiles;
		updateRedTiles(p1Tiles);
		updateBlueTiles(p2Tiles);
		switchColorText();
		_eventBus.onTilePlace += onTilePlace;
		_eventBus.onTurnChange += onTurnChange;
		_eventBus.onPhaseStart += phaseSwitched;
		_eventBus.onBoardReset += resetUi;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	private void resetUi () 
	{
		updateRedTiles(sceneManager.p1Tiles);
		updateBlueTiles(sceneManager.p2Tiles);
		currentPlayer = manager.playerTurn();
		switchColorText();

	}
	private void onTurnChange(Piece.Color turn)
	{
		if (turn != currentPlayer) {
			switchColorText();
			currentPlayer = turn;
		}
	}
	private void updateRedTiles() {
		redTiles.Text = "Player 1 has: [color=#FA003F] " + 
								(manager.playerTileCount(Piece.Color.PLAYER_1) - 1) + 
								"[/color] tiles";
	}
	private void updateBlueTiles () {
		blueTiles.Text = "Player 2 has: [color=dodger_blue] " + 
								(manager.playerTileCount(Piece.Color.PLAYER_2) - 1) + 
								"[/color] tiles";
	}

	private void updateRedTiles(int tiles) {
		redTiles.Text = "Player 1 has: [color=#FA003F] " + 
								tiles + 
								"[/color] tiles";
	}
	private void updateBlueTiles (int tiles) {
		blueTiles.Text = "Player 2 has: [color=dodger_blue] " + 
								tiles + 
								"[/color] tiles";
	}
	
	
	private void onTilePlace()
	{
		switch(currentPlayer) {
			case Piece.Color.PLAYER_1:
				updateRedTiles();
				break;
			case Piece.Color.PLAYER_2:
				updateBlueTiles();
				break;
			default:
					break;
			}
	}
	private void phaseSwitched(BoardManager.GamePhase phase) 
	{
		switch (phase)
		{
			case BoardManager.GamePhase.MOVE:
				redTiles.Visible = false;
				blueTiles.Visible = false;
				break;
			case BoardManager.GamePhase.PLACE:
				redTiles.Visible = true;
				blueTiles.Visible = true;
				break;
			default:
				break;
		}
	}
	public override void _Process(double delta)
	{
	}
	private void switchColorText () {
		switch(currentPlayer) {
			case Piece.Color.PLAYER_1:
				turnLabel.Text = "Current Turn is: [color=#FA003F] Player 1 [/color]";
				break;
			case Piece.Color.PLAYER_2:
				turnLabel.Text = "Current Turn is: [color=dodger_blue] Player 2 [/color]";
				break;
			default:
					break;
			}
	}
}
