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
	public RichTextLabel phaseLabel;

	[Export]
	public RichTextLabel redTiles;

	[Export]
	public RichTextLabel blueTiles;

	private PlayerColor currentPlayer;

	private BoardManager manager;
	private int p1Tiles, p2Tiles;
	private EventBus _eventBus; 
	private SceneManager sceneManager;

	public void hideAll () {
		redTiles.Hide();
		blueTiles.Hide();
		turnLabel.Hide();
		phaseLabel.Hide();
		GetNode<Button>("CanvasLayer/SaveButton").Visible = false;
		GetNode<Button>("CanvasLayer/LoadButton").Visible = false;
		GetNode<Button>("CanvasLayer/ResetButton").Visible = false;
	}
	
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
		switchPhaseText();
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
	private void onTurnChange(PlayerColor turn)
	{
		currentPlayer = turn;
		switchColorText();
		
	}
	public void switchPhaseText() {
		switch (manager.phase()) {
			case GamePhase.PLACE:
				phaseLabel.Text = "Placing Phase";
				break;
			case GamePhase.MOVE:
				phaseLabel.Text = "Movement Phase";
				break;
			default:
				break;
		}
	}
	public void updateRedTiles() {
		redTiles.Text = "Player 1 has: [color=#FA003F] " + 
								(manager.playerTileCount(PlayerColor.PLAYER_1) - 1) + 
								"[/color] tiles";
	}
	public void updateBlueTiles () {
		blueTiles.Text = "Player 2 has: [color=dodger_blue] " + 
								(manager.playerTileCount(PlayerColor.PLAYER_2) - 1) + 
								"[/color] tiles";
	}

	public void updateRedTiles(int tiles) {
		redTiles.Text = "Player 1 has: [color=#FA003F] " + 
								tiles + 
								"[/color] tiles";
	}
	public void updateBlueTiles (int tiles) {
		blueTiles.Text = "Player 2 has: [color=dodger_blue] " + 
								tiles + 
								"[/color] tiles";
	}
	
	
	private void onTilePlace()
	{
		switch(currentPlayer) {
			case PlayerColor.PLAYER_1:
				updateRedTiles();
				break;
			case PlayerColor.PLAYER_2:
				updateBlueTiles();
				break;
			default:
				break;
			}
	}
	private void phaseSwitched(GamePhase phase) 
	{
		// currentPlayer = manager.playerTurn();
		// switchColorText();
		switchPhaseText();
		switch (phase)
		{
			case GamePhase.MOVE:
				redTiles.Visible = false;
				blueTiles.Visible = false;
				break;
			case GamePhase.PLACE:
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
			case PlayerColor.PLAYER_1:
				turnLabel.Text = "[color=#FA003F] Player 1 [/color]'s turn";
				break;
			case PlayerColor.PLAYER_2:
				turnLabel.Text = "[color=dodger_blue] Player 2 [/color]'s turn";
				break;
			default:
					break;
			}
	}
}
