using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public class GameSaver : FileSaver
{
	// Called when the node enters;
    public GameSaver(){}
	public void SaveGame(Board board, Piece.Color turn, int p1Tiles, 
                            int p2Tiles, BoardManager.GamePhase phase, String path) {
		
		FileAccess gameFile = FileAccess.Open(path, FileAccess.ModeFlags.Write);
		int[] size = board.size();
		int currTurn = (turn == Piece.Color.PLAYER_1) ? 1 : 2; 
		string currPhase = (phase == BoardManager.GamePhase.MOVE) ? "move" : "place"; 
		gameFile.StoreLine("rows:"+ size[0] + ", cols:" + size[1]);
		gameFile.StoreLine("turn:"+ currTurn + ", p1:" + p1Tiles + ", p2:" 
								+ p2Tiles + ", phase:" + currPhase );
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
    public (SteppingStonesBoard, Piece.Color, int, int, BoardManager.GamePhase) 
		LoadGame(String fileName) {
		if (!FileAccess.FileExists(fileName)) {
			throw new System.IO.FileNotFoundException();
		}
		FileAccess gameFile = FileAccess.Open(fileName, FileAccess.ModeFlags.Read);
		int[] size = gameFile.GetLine().Split(", ").
							Select(s => int.Parse(s.Substring(s.IndexOf(":") + 1))).ToArray();
		int rowCount = size[0]; 
		int columnCount = size[1];
		IList<string> inputs = gameFile.GetLine().Split(", ").Select(s => s.Substring(s.IndexOf(":") + 1)).ToList();
		Piece.Color turn = (inputs[0] == "1") ? Piece.Color.PLAYER_1 : Piece.Color.PLAYER_2;
		int p1Tiles = int.Parse(inputs[1]);
		int p2Tiles = int.Parse(inputs[2]);
		BoardManager.GamePhase phase = (inputs[3] == "move") ? BoardManager.GamePhase.MOVE : BoardManager.GamePhase.PLACE;
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
		return (board, turn, p1Tiles, p2Tiles, phase);
	} 
}


