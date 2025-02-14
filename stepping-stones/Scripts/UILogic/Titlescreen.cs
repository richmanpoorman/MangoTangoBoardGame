using Godot;
using System;

public partial class Titlescreen : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	private FileDialog loadBox;
	private FileSaver saver = new GameSaver();
	public override void _Ready()
	{
	}
	private void OnStartButtonPressed() {
		//TODO: Ask about different grid sizes
		SceneManager.Instance.goToMainBoard(new GridSteppingStonesBoard(5, 7));
	}
	private void OnNewGameButtonPressed() {
		SceneManager.Instance.goToMainBoard(new GridSteppingStonesBoard(5, 7));
	}
	private void OnLoadGameButtonPressed() {
		loadBox.Popup();
	}
	private void OnMainLoadFileSelected(String path) {
		SceneManager.Instance.goToMainBoard(saver.LoadGame(path));
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
