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
			ISceneRunner runner = ISceneRunner.Load("res://Scenes/Board_Scene.tscn");
			BoardManager manager = (BoardManager)runner.FindChild("BoardManager");//game.manager;
			GridSteppingStonesBoard board = new GridSteppingStonesBoard(5, 7);
			Tile tile = new Tile(Piece.Color.PLAYER_1);
			board.placeTile(tile, Location.at(2, 3));
			manager.setBoard(board);
			manager.setTileCount(Piece.Color.PLAYER_1, 5);
			Assertions.AssertThat(manager.playerTileCount(Piece.Color.PLAYER_1)).IsEqual(5);
			manager.setTileCount(Piece.Color.PLAYER_2, 6);
			Assertions.AssertThat(manager.playerTileCount(Piece.Color.PLAYER_2)).IsEqual(6);
		}

		[TestCase]
		public void turnColorPlaceTestP1(){
			ISceneRunner runner = ISceneRunner.Load("res://Scenes/Board_Scene.tscn");
			BoardManager manager = (BoardManager)runner.FindChild("BoardManager");//game.manager;
			GridSteppingStonesBoard board = new GridSteppingStonesBoard(5, 7);
			manager.setBoard(board);
			
			Piece.Color turn1 = manager.playerTurn();
			Assertions.AssertThat(manager.board().tileAt(Location.at(1,2))).IsEqual(null);
			manager.onCellSelection(turn1, 1, 2);
			Assertions.AssertThat(manager.board().tileAt(Location.at(1,2)).color()).IsEqual(turn1);
			Assertions.AssertThat(manager.playerTurn()).IsNotEqual(turn1);

			//check that nothing happens when p1 tries to go
			Assertions.AssertThat(manager.board().tileAt(Location.at(2,2))).IsEqual(null);
			manager.onCellSelection(turn1, 2, 2);
			Assertions.AssertThat(manager.board().tileAt(Location.at(2,2))).IsEqual(null);
			Assertions.AssertThat(manager.playerTurn()).IsNotEqual(turn1);

			

		}

	// 	[TestCase]
	// 	public void boundPlaceTest(){}

	// 	public void noMoveOutOfBounds(){}

	// 	public void noMoveWrongColor(){}

	// 	public void scoutlessPushLeft(){}

	// 	public void blockWrongScoutlessPushLeft(){}

	// 	public void scoutlessPushRight(){}

	// 	public void blockWrongScoutlessPushRight(){}

	// 	public void scoutlessPushUp(){}

	// 	public void blockWrongScoutlessPushUp(){}

	// 	public void scoutlessPushDown(){}

	// 	public void blockWrongScoutlessPushDown(){}

	// 	public void scoutMoveToOwn(){}

	// 	public void blockMoveScoutToEmpty(){}

	// 	public void blockMoveScoutToOther(){}

	}
}
