using Godot;
using System;

public partial class NeworLoad : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	private FileDialog loadBox;

	[Export]
	private Button startButton;
	[Export]
	private Button backButton;
	[Export]
	private Button newButton;
	[Export]
	private Button loadButton;

	[Export]
	private Button makeRoomButton, joinRoomButton; 
	[Export]
	private LineEdit roomJoiner; 

	[Export]
	private TabContainer tabs;
	private SceneManager sceneManager;
	private FileSaver saver = new GameSaver();

	[Export]
	private Node2D roomCodePopup;
	[Export]
	private Button makeGameButton;


	private EventBus _bus; 

	private int width = 5;
	private int length = 7;

	private int numTiles = 4;
	

	private void OnNewGameButtonPressed() 
	{
		tabs.Visible = true;
		startButton.Visible = true;
		backButton.Visible = true;
		newButton.Visible = false;
		loadButton.Visible = false;
		makeRoomButton.Visible = false; 
		joinRoomButton.Visible = false; 
		roomJoiner.Visible = false; 
	}
	private void OnStartGameButtonPressed() 
	{
		sceneManager.newGame = true;
		sceneManager.goToMainBoard(new GridSteppingStonesBoard(width, length), numTiles);
	}
	private void OnLoadGameButtonPressed() 
	{
		loadBox.Popup();
	}

	private void OnMainLoadFileSelected(String path) {

		sceneManager.newGame = false;
		sceneManager.goToMainBoard(saver.LoadGame(path).ToTuple());
	}

	private void OnWidthBoxValueChanged(float value)
	{
		width = (int)value;

	}

	private void OnCloseButtonPressed() 
	{
		tabs.Visible = false;
	}
	private void OnLengthBoxValueChanged(float value)
	{
		length = (int)value;
	}
	private void OnNumTilesBoxValueChanged(float value) 
	{
		numTiles = (int)value;
	}
	
	private void onJoinRoomPressed() {
		roomJoiner.Visible = true; 
	}

	private void onMakeGamePressed() {
		_bus.EmitSignal(EventBus.SignalName.onMakeRoom); 
		roomCodePopup.Visible = true;
		tabs.Visible = false;
		makeGameButton.Visible = false;
		backButton.Visible = false;
	}

	private void onMakeRoomPressed() {
		tabs.Visible = true;
		makeGameButton.Visible = true;
		backButton.Visible = true;
		newButton.Visible = false;
		loadButton.Visible = false;
		makeRoomButton.Visible = false; 
		joinRoomButton.Visible = false; 
		roomJoiner.Visible = false; 
	}

	private void onMakeRoomCodeReadyPressed() {
		sceneManager.newGame = true; 
		// TODO:: Send board information to the server   
		// sceneManager.goToMainBoard(new GridSteppingStonesBoard(width, length), numTiles);
		sceneManager.p1Tiles = numTiles;
		sceneManager.p2Tiles = numTiles;
		sceneManager.board = new GridSteppingStonesBoard(width, length);
		sceneManager.turn = PlayerColor.PLAYER_1;
		_bus.EmitSignal(EventBus.SignalName.onSetGameToSceneManagerRequest);
		
	}

	private void onJoinCodeEntered(string roomCode) {
		roomJoiner.Clear();
		roomJoiner.Visible = false; 
		_bus.EmitSignal(EventBus.SignalName.onJoinRoom, roomCode.Trim().ToUpper());

		// TODO:: Change to listen for the information from the online server 
		// sceneManager.goToMainBoard(new GridSteppingStonesBoard(width, length), numTiles);
		// _bus.EmitSignal(EventBus.SignalName.onSetGameToSceneManagerRequest);
	}

	public override void _Ready()
	{
		sceneManager = SceneManager.Instance;
		_bus = EventBus.Bus; 
	}

	
	private void OnBackButtonPressed() 
	{
		tabs.Visible = false;
		startButton.Visible = false;
		backButton.Visible = false;
		makeGameButton.Visible = false;
		newButton.Visible = true;
		loadButton.Visible = true;
		makeRoomButton.Visible = true; 
		joinRoomButton.Visible = true; 
	}

	
}
