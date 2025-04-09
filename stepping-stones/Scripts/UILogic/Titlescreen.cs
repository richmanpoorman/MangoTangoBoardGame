using Godot;
using System;

public partial class Titlescreen : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	private FileDialog loadBox;
	private FileSaver saver = new GameSaver();
	private SceneManager sceneManager;
	public override void _Ready()
	{
		sceneManager = SceneManager.Instance;
	}
	private void OnStartButtonPressed() {
		//TODO: Ask about different grid sizes
		sceneManager.goToMainBoard(new GridSteppingStonesBoard(5, 7));
	}

	private void OnMainLoadFileSelected(String path) {
		sceneManager.goToMainBoard(saver.LoadGame(path).ToTuple());
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
