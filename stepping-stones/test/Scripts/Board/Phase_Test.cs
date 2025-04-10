// GdUnit generated TestSuite
using Godot;
using GdUnit4;
using System.Threading.Tasks;

namespace GdUnitDefaultTestNamespace
{
	using static Assertions;
	using static Utils;

	[TestSuite]
	public class Phase_Test
	{
		private ISceneRunner runner;
		private BoardManager manager;
		private GridSteppingStonesBoard board;
		private Piece.Color p1, p2;



		[BeforeTest]
		public void setup(){
			runner = ISceneRunner.Load("res://Scenes/Board_Scene.tscn");
			manager = (BoardManager)runner.FindChild("BoardManager");
			board = new GridSteppingStonesBoard(5, 7);
			manager.setBoard(board);
			p1 = Piece.Color.PLAYER_1;
            p2 = Piece.Color.PLAYER_2;

		}
		// TestSuite generated from
		private const string sourceClazzPath = "./../../../Scripts/UILogic/MainGame.cs";
		
		[TestCase]
		public void placeToMoveIsAutomatic(){
			manager.setTileCount(p1, 1);
			manager.setTileCount(p2, 1);
			Assertions.AssertThat(manager.phase()).IsEqual(BoardManager.GamePhase.PLACE);

			manager.onCellSelection(p1, 0, 0);
			manager.onCellSelection(p2, 0, 1);
			Assertions.AssertThat(manager.phase()).IsEqual(BoardManager.GamePhase.MOVE);


		}

		[TestCase]
		public void placeToMoveTurnOrderP1()
		{
			manager.setTileCount(p1, 1);
			manager.setTileCount(p2, 1);
			Assertions.AssertThat(manager.phase()).IsEqual(BoardManager.GamePhase.PLACE);
			Assertions.AssertThat(manager.playerTurn()).IsEqual(p1);

			manager.onCellSelection(p1, 0, 0);
			manager.onCellSelection(p2, 0, 1);
			Assertions.AssertThat(manager.phase()).IsEqual(BoardManager.GamePhase.MOVE);
			Assertions.AssertThat(manager.playerTurn()).IsEqual(p1);

		}

		[TestCase]
		public void placeToMoveTurnOrderP2()
		{
			manager.setTileCount(p1, 1);
			manager.setTileCount(p2, 1);
			manager.setTurn(p2);
			Assertions.AssertThat(manager.phase()).IsEqual(BoardManager.GamePhase.PLACE);
			Assertions.AssertThat(manager.playerTurn()).IsEqual(p2);

			manager.onCellSelection(p2, 0, 1);
			manager.onCellSelection(p1, 0, 0);
			Assertions.AssertThat(manager.phase()).IsEqual(BoardManager.GamePhase.MOVE);
			Assertions.AssertThat(manager.playerTurn()).IsEqual(p2);

		}

		[TestCase]
		public void placeToMoveTilePlacement(){
			manager.setTileCount(p1, 2);
			manager.setTileCount(p2, 2);
			Assertions.AssertThat(manager.phase()).IsEqual(BoardManager.GamePhase.PLACE);

			manager.onCellSelection(p1, 0, 0);
			manager.onCellSelection(p2, 0, 1);
			manager.onCellSelection(p1, 0, 2);
			manager.onCellSelection(p2, 0, 3);
			Assertions.AssertThat(manager.phase()).IsEqual(BoardManager.GamePhase.MOVE);


			for (int i = 0; i < 5; i ++){
				for (int j = 0; j < 7; i++){
					Location currLoc = Location.at(i, j);
					Tile currTile = manager.board().tileAt(currLoc);
					if ((i == 0 && j < 4)){
						if (j%2 == 0){
							Assertions.AssertThat(currTile.color()).IsEqual(p1);
						} else {
							Assertions.AssertThat(currTile.color()).IsEqual(p2);
						}
					} else if (i == 2 && (j == 1 || j == 5)){
						Assertions.AssertThat(currTile).IsNotNull();
					} else {
						Assertions.AssertThat(currTile).IsNull();
					}
				}
			}

		}
		

		//[TestCase]
		//public void restartToPlaceNoMove(){}

		//[TestCase]
		//public void restartToPlaceNoTilesPlaced(){}
		
		//[TestCase]
		//public void restartToPlaceTileCountMaintained(){}

		//[TestCase]
		//public void restartToPlaceIsAutomatic(){}

		//[TestCase]
		//public void moveToWinScoutPass(){}

		//[TestCase]
		//public void moveToWinScoutKnockout(){}

		//[TestCase]
		//public void moveToWinScoutKnockoutAndPass(){}

		//[TestCase]
		//public void moveToWinPushedIntoZone(){}

	}
}
