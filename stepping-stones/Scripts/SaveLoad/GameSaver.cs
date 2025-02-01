using Godot;
using System;
public class GameSaver : FileSaver
{
	// Called when the node enters;
    public GameSaver(){}
	public void SaveGame(Board board) {
		
		String gameNumber;
		if (!FileAccess.FileExists("user://SaveMetaData.step")) {
			FileAccess metaFile = FileAccess.Open("user://SaveMetaData.step", FileAccess.ModeFlags.Write);
			gameNumber = "0";
			metaFile.StoreLine(gameNumber);
			metaFile.Close();
		} else {
			FileAccess metaFile = FileAccess.Open("user://SaveMetaData.step", FileAccess.ModeFlags.ReadWrite);
			String temp = metaFile.GetLine();
			if (temp == "") {
				GD.Print("temp is empty");
				temp = "0"; 
			} else {
				GD.Print("temp is: " + temp);
			}
			
			gameNumber = (int.Parse(temp) + 1).ToString();
			GD.Print("game num is: " + gameNumber);
			metaFile.StoreLine(gameNumber);
			metaFile.Close();
		}
		
		FileAccess gameFile = FileAccess.Open("user://Game" + gameNumber + ".step", FileAccess.ModeFlags.Write);
		int[] size = board.size();
		gameFile.StoreLine(size[0] + " " + size[1]);
		#nullable enable
		for (int i = 0; i < size[0]; i++) for (int j = 0; j < size[1]; j++) {
			String entry = "";
			Tile? currTile = board.tileAt(new Location(i ,j)); 
			if (currTile != null) {
				char color = (currTile.color() == Piece.Color.PLAYER_1) ? 'r' : 'b';
				entry = "" + (char)('a' + i) + (j + 1) + '=' + color + 't';
				Scout? currScout = board.scoutAt(new Location(i,j));
				if (currScout != null) {
					entry += 's';
				}
			}
			if (entry != "") {
				gameFile.StoreLine(entry);
			}
		}
		gameFile.Close();
		#nullable disable
	}
    public SteppingStonesBoard LoadGame(String fileName) {
		if (!FileAccess.FileExists(fileName)) {
			throw new System.IO.FileNotFoundException();
		}
		FileAccess gameFile = FileAccess.Open(fileName, FileAccess.ModeFlags.Read);
		int[] size = Array.ConvertAll(gameFile.GetLine().Split(' '), int.Parse);
		SteppingStonesBoard board = new GridSteppingStonesBoard(size[1], size[0]);
		GD.Print("board size is: " + board.size()[0] + board.size()[1]);
		board.removeTile(new Location(size[1] / 2, 1));
		board.removeTile(new Location(size[1] / 2, size[0] - 2));
		board.removeScout(new Location(size[1] / 2, 1));
		board.removeScout(new Location(size[1] / 2, size[0] - 2));
		while (gameFile.GetPosition() < gameFile.GetLength()) {
			String line = gameFile.GetLine();
			int row = line[0] - 'a';
			int col = line[1] - '1';
			GD.Print("row is: " + row + ", col is: " + col);
			Piece.Color color = (line[3] == 'r') ? Piece.Color.PLAYER_1 : Piece.Color.PLAYER_2;
			Tile currTile = new Tile(color);
			board.addTile(currTile, new Location(row, col));
			if (line.Length == 5) {
				board.addScout(new Scout(color), new Location(row, col));
			}
		}	
		return board;
	}
}


