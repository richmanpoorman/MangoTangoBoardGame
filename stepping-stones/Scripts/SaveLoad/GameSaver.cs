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
		} else {
			FileAccess metaFile = FileAccess.Open("user://SaveMetaData.step", FileAccess.ModeFlags.Write);
			gameNumber = metaFile.GetLine() ;
		}
		FileAccess gameFile = FileAccess.Open("user://Game" + gameNumber + ".step", FileAccess.ModeFlags.Write);
		int[] size = board.size();
		gameFile.StoreLine(size[0] + " " + size[1]);
		#nullable enable
		for (int i = 0; i < size[0]; i++) for (int j = 0; j < size[1]; j++) {
			String? entry = null;
			Tile? currTile = board.tileAt(new Board.Position(i ,j)); 
			if (currTile != null) {
				char color = (currTile.color() == Piece.Color.PLAYER_1) ? 'r' : 'b';
				entry = "" + ('a' + i) + j + '=' + color + 't';
				Scout? currScout = board.scoutAt(new Board.Position(i,j));
				if (currScout != null) {
					entry += 's';
				}
			}
			if (entry != null) {
				gameFile.StoreLine(entry);
			}
		}
		#nullable disable
	}
    public Board LoadGame(String fileName) {
		if (!FileAccess.FileExists(fileName)) {
			throw new System.IO.FileNotFoundException();
		}
		FileAccess gameFile = FileAccess.Open(fileName, FileAccess.ModeFlags.Read);
		int[] size = Array.ConvertAll(gameFile.GetLine().Split(' '), int.Parse);
		Board board = new GridBoard(size[0], size[1]);
		board.removeTile(new Board.Position(size[1] / 2, 1));
		board.removeTile(new Board.Position(size[1] / 2, size[0] - 2));
		board.removeScout(new Board.Position(size[1] / 2, 1));
		board.removeScout(new Board.Position(size[1] / 2, size[0] - 2));
		while (gameFile.GetPosition() < gameFile.GetLength()) {
			String line = gameFile.GetLine();
			int row = line[0] - 'a';
			int col = line[1] - '0';
			Piece.Color color = (line[3] == 'r') ? Piece.Color.PLAYER_1 : Piece.Color.PLAYER_2;
			Tile currTile = new Tile(color);
			board.addTile(currTile, new Board.Position(row, col));
			if (line.Length == 5) {
				board.addScout(new Scout(color), new Board.Position(row, col));
			}
		}	
		return board;
	}
}


