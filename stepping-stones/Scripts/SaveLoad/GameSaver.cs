using Godot;
using System;
public class GameSaver : FileSaver
{
	// Called when the node enters;
    public GameSaver(){}
	public void SaveGame(Board board, String path) {
		
		FileAccess gameFile = FileAccess.Open(path, FileAccess.ModeFlags.Write);
		int[] size = board.size();
		gameFile.StoreLine(size[0] + " " + size[1]);
		#nullable enable
		for (int i = 0; i < size[0]; i++) for (int j = 0; j < size[1]; j++) {
			String entry = "";
			Tile? currTile = board.tileAt(Location.at(i ,j)); 
			if (currTile != null) {
				char color = (currTile.color() == Piece.Color.PLAYER_1) ? 'r' : 'b';
				entry = "" + (char)('a' + i) + (j + 1) + '=' + color + 't';
				Scout? currScout = board.scoutAt(Location.at(i,j));
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
		int rowCount = size[0]; 
		int columnCount = size[1]; 
		GD.Print("file size is: " + rowCount + " x " + columnCount);
		SteppingStonesBoard board = new GridSteppingStonesBoard(size[0], size[1]);
		GD.Print("board size is: " + board.size()[0] + " x " + board.size()[1]);
		board.removeTile(Location.at(rowCount / 2, 1));
		board.removeTile(Location.at(rowCount / 2, columnCount - 2));
		board.removeScout(Location.at(rowCount / 2, 1));
		board.removeScout(Location.at(rowCount / 2, columnCount - 2));
		while (gameFile.GetPosition() < gameFile.GetLength()) {
			String line = gameFile.GetLine();
			int row = line[0] - 'a';
			int col = line[1] - '1';
			GD.Print("row is: " + row + ", col is: " + col);
			Piece.Color color = (line[3] == 'r') ? Piece.Color.PLAYER_1 : Piece.Color.PLAYER_2;
			Tile currTile = new Tile(color);
			board.addTile(currTile, Location.at(row, col));
			if (line.EndsWith("s")) {
				board.addScout(new Scout(color), Location.at(row, col));
			}
		}	
		return board;
	}
}


