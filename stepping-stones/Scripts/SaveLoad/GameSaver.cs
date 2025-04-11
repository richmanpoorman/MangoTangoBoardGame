using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
public class GameSaver : FileSaver
{
	// Called when the node enters;
    public GameSaver(){}
	public void SaveGame(Board board, PlayerColor turn, int p1Tiles, 
                            int p2Tiles, GamePhase phase, String path) {
		
		FileAccess gameFile = FileAccess.Open(path, FileAccess.ModeFlags.Write);
		int[] size = board.size();
		int currTurn = (turn == PlayerColor.PLAYER_1) ? 1 : 2; 
		string currPhase = (phase == GamePhase.MOVE) ? "move" : "place"; 
		gameFile.StoreLine("rows:"+ size[0] + ", cols:" + size[1]);
		gameFile.StoreLine("turn:"+ currTurn + ", p1:" + p1Tiles + ", p2:" 
								+ p2Tiles + ", phase:" + currPhase );
		#nullable enable
		for (int i = 0; i < size[0]; i++) for (int j = 0; j < size[1]; j++) {
			String entry = "";
			Tile? currTile = board.tileAt(Location.at(i ,j)); 
			if (currTile != null) {
				char color = (currTile.color() == PlayerColor.PLAYER_1) ? 'r' : 'b';
				string col = (j < 10) ?  string.Concat("0", (char)(j + '1')) : j.ToString(); 
				entry = "" + (char)('a' + i) + col + '=' + color + 't';
				Scout? currScout = board.scoutAt(Location.at(i,j));
				if (currScout != null) {
					entry += 's';
				}
			}
			if (entry != "") {
				gameFile.StoreLine(entry);
				GD.Print(entry);
			}
		}
		gameFile.Close();
		#nullable disable
	}
    public (SteppingStonesBoard, PlayerColor, int, int, GamePhase) 
		LoadGame(String fileName) {
		if (!FileAccess.FileExists(fileName)) {
			throw new System.IO.FileNotFoundException();
		}
		// GD.Print(fileName); TODO: LEADING 0
		FileAccess gameFile = FileAccess.Open(fileName, FileAccess.ModeFlags.Read);
		int[] size = gameFile.GetLine().Split(", ").
							Select(s => int.Parse(s.Substring(s.IndexOf(":") + 1))).ToArray();
		int rowCount = size[0]; 
		int columnCount = size[1];
		IList<string> inputs = gameFile.GetLine().Split(", ").Select(s => s.Substring(s.IndexOf(":") + 1)).ToList();
		PlayerColor turn = (inputs[0] == "1") ? PlayerColor.PLAYER_1 : PlayerColor.PLAYER_2;
		int p1Tiles = int.Parse(inputs[1]);
		int p2Tiles = int.Parse(inputs[2]);
		GamePhase phase = (inputs[3] == "move") ? GamePhase.MOVE : GamePhase.PLACE;
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
			int col = int.Parse(line.Substring(1, 2)) - 1;
			GD.Print("row is: " + row + ", col is: " + col);
			PlayerColor color = (line[4] == 'r') ? PlayerColor.PLAYER_1 : PlayerColor.PLAYER_2;
			Tile currTile = new Tile(color);
			board.addTile(currTile, Location.at(row, col));
			if (line.EndsWith("s")) {
				board.addScout(new Scout(color), Location.at(row, col));
			}
		}
		return (board, turn, p1Tiles, p2Tiles, phase);
	} 
}


