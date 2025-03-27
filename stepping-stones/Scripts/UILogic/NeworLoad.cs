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
	private TabContainer tabs;
	private SceneManager sceneManager;
	private FileSaver saver = new GameSaver();

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
	
	public override void _Ready()
	{
		sceneManager = SceneManager.Instance;
	}

	
	private void OnBackButtonPressed() 
	{
		tabs.Visible = false;
		startButton.Visible = false;
		backButton.Visible = false;
		newButton.Visible = true;
		loadButton.Visible = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
