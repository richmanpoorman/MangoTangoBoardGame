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
		display = GetNode<BoardDisplay>("Main/BoardManager/Board");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnUISaveGame() {
		// GD.Print("Game Saved");
		// Board board = manager.board();
		// int[] size = board.size();
		// GD.Print("size 0: " + size[0]);
		// GD.Print("size 1: " + size[1]);
		// #nullable enable
		// for (int i = 0; i < size[0]; i++) {
		// 	for (int j = 0; j < size[1]; j++) {
		// 		String entry = "";
		// 		Tile? currTile = board.tileAt(new Location(i ,j)); 
		// 		if (currTile != null) {
		// 			char color = (currTile.color() == Piece.Color.PLAYER_1) ? 'r' : 'b';
		// 			entry = "" + (char)('a' + i) + (j + 1)+ '=' + color + 't';
		// 			Scout? currScout = board.scoutAt(new Location(i,j));
		// 			if (currScout != null) {
		// 				entry += 's';
		// 			}
		// 		}
		// 		if(entry != "") {	
		// 			GD.Print(entry);
		// 		}
		// 	} 
		// GD.Print("###");
		// }
		// #nullable disable
		saver.SaveGame(manager.board());

	}
		public void OnUILoadGame() {
			GD.Print("Game Loaded");
			manager.setBoard(saver.LoadGame("user://Game1.step"));
			display.updateDisplay();
	}
}
