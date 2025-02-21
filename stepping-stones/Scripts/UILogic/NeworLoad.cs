using Godot;
using System;

public partial class NeworLoad : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	private FileDialog loadBox;
	private SceneManager sceneManager;
	private FileSaver saver = new GameSaver();
	private void OnNewGameButtonPressed() 
	{
		sceneManager.goToMainBoard(new GridSteppingStonesBoard(9, 9));
	}
	private void OnLoadGameButtonPressed() 
	{
		loadBox.Popup();
	}

	private void OnMainLoadFileSelected(String path) {
		sceneManager.goToMainBoard(saver.LoadGame(path));
	}
	
	public override void _Ready()
	{
		sceneManager = SceneManager.Instance;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
