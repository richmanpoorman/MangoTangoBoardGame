using Godot;
using System;
using GdUnit4;
using System.Data;
using static GdUnit4.Assertions;
using System.Runtime.InteropServices;

[TestSuite]
public class SaveLoadTest {
	private GameSaver saver;
	private string testFile = "user://savertest.step";
	[Before]
	public void mkSaver() {
		saver = new GameSaver();
	}
	[TestCase]
	public void saveAndLoad() {
		SteppingStonesBoard board = new GridSteppingStonesBoard(5, 7);
		saver.SaveGame(board, Piece.Color.PLAYER_1, 3, 3,
		BoardManager.GamePhase.PLACE, "user://savertest.step");
		(SteppingStonesBoard lboard, Piece.Color turn, int p1Tiles, 
			int p2Tiles, BoardManager.GamePhase phase) = saver.LoadGame(testFile);
		AssertThat(turn).IsEqual(Piece.Color.PLAYER_1);
		AssertThat(p1Tiles).IsEqual(3);
		AssertThat(p2Tiles).IsEqual(3);
		AssertThat(phase).IsEqual(BoardManager.GamePhase.PLACE);
		AssertThat(lboard.size()).IsEqual(board.size());
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 7; j++) {
				GD.PushError("i is: " + i + " j is: " + j);
				if (board.tileAt(Location.at(i, j)) == null) {
					AssertThat(lboard.tileAt(Location.at(i, j))).IsNull();
				} else {
					AssertThat(lboard.tileAt(Location.at(i, j)).color())
					.IsEqual(board.tileAt(Location.at(i, j)).color());
				}
				if (board.scoutAt(Location.at(i, j)) == null) {
					AssertThat(lboard.scoutAt(Location.at(i, j))).IsNull();
				} else {
					AssertThat(lboard.scoutAt(Location.at(i, j)).color())
					.IsEqual(board.scoutAt(Location.at(i, j)).color());
				}
			}
		}
	}
	}
