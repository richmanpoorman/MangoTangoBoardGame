// GdUnit generated TestSuite
using Godot;
using GdUnit4;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Xml.Schema;

namespace GdUnitDefaultTestNamespace
{
	using static Assertions;
	using static Utils;

	[TestSuite]
	public class GridSteppingStonesBoardTest
	{
		// TestSuite generated from
		private const string sourceClazzPath = "./../../../Scripts/Board/GridSteppingStonesBoard.cs";
		[TestCase]
		public void placeTile()
		{
			GridSteppingStonesBoard board = new GridSteppingStonesBoard(5, 7);
			Tile tile = new Tile(Piece.Color.PLAYER_1);
			board.placeTile(tile, Location.at(2, 3));
			Assertions.AssertThat(board.tileAt(Location.at(2,3))).Equals(tile);
			
			//Make sure that cannot override tile with new tile			
			board.placeTile(new Tile(Piece.Color.PLAYER_2), Location.at(2, 3));
			Assertions.AssertThat(board.tileAt(Location.at(2,3))).Equals(tile);			

		}
		
		
	}
}
