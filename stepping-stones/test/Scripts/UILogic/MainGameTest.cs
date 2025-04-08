// GdUnit generated TestSuite
using Godot;
using GdUnit4;
using System.Threading.Tasks;

namespace GdUnitDefaultTestNamespace
{
	using static Assertions;
	using static Utils;

	[TestSuite]
	public class MainGameTest
	{
		// TestSuite generated from
		private const string sourceClazzPath = "/Users/janebrockett/Documents/GitHub/MangoTangoBoardGame/stepping-stones/Scripts/UILogic/MainGame.cs";
		[TestCase]
		public async Task setPlayerTile()
		{
			ISceneRunner runner = ISceneRunner.Load("res://Scenes/Main.tscn");
			BoardManager manager = (BoardManager)runner.FindChild("BoardManager");//game.manager;
			GridSteppingStonesBoard board = new GridSteppingStonesBoard(5, 7);
			Tile tile = new Tile(Piece.Color.PLAYER_1);
			board.placeTile(tile, Location.at(2, 3));
			manager.setBoard(board);
			manager.setTileCount(Piece.Color.PLAYER_1, 5);
			Assertions.AssertThat(manager.playerTileCount(Piece.Color.PLAYER_1)).IsEqual(5);
			}
	}
}
