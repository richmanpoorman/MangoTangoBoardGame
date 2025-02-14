using Godot;
using System;

public partial class MainGame : Node2D
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	public BoardManager manager;
	public BoardDisplay display;
	private FileSaver saver =  new GameSaver();

	public override void _Ready()
	{
		manager = GetNode<BoardManager>("Main/BoardManager");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnUISaveGame(String path) {
		saver.SaveGame(manager.board(), path);
	}
	public void OnUIResetGame() {
		manager.onRestart();
	}
	public void OnUILoadGame(String path) {
		GD.Print("Game Loaded");
		manager.setBoard(saver.LoadGame(path));
		Board board = manager.board();
		int[] size = manager.board().size();
	}
}
