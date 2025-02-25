using Godot;
using System;

public partial class NeworLoad : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	private FileDialog loadBox;

	[Export]
	private HBoxContainer hBox;
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

	private void OnNewGameButtonPressed() 
	{
		tabs.Visible = true;
		hBox.Visible = true;
		newButton.Visible = false;
		loadButton.Visible = false;
	}
	private void OnStartGameButtonPressed() 
	{
		sceneManager.goToMainBoard(new GridSteppingStonesBoard(width, length));
	}
	private void OnLoadGameButtonPressed() 
	{
		loadBox.Popup();
	}

	private void OnMainLoadFileSelected(String path) {
		sceneManager.goToMainBoard(saver.LoadGame(path));
	}

	private void OnWidthBoxValueChanged(float value)
	{
		width = (int)value;

	}
	private void OnLengthBoxValueChanged(float value)
	{
		length = (int)value;
	}
	
	public override void _Ready()
	{
		sceneManager = SceneManager.Instance;
	}

	
	private void OnBackButtonPressed() 
	{
		tabs.Visible = false;
		hBox.Visible = false;
		newButton.Visible = true;
		loadButton.Visible = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
