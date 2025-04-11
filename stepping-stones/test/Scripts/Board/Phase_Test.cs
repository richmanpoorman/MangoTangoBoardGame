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
		private EventBus _eventBus = EventBus.Bus;



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

			manager.onCellSelection(p1, 0, 2);

			Assertions.AssertThat(manager.phase()).IsEqual(BoardManager.GamePhase.MOVE);


		}

		[TestCase]
		public void placeToMoveTurnOrderP1()
		{
			manager.setTileCount(p1, 0);
			manager.setTileCount(p2, 0);
			Assertions.AssertThat(manager.phase()).IsEqual(BoardManager.GamePhase.PLACE);
			Assertions.AssertThat(manager.playerTurn()).IsEqual(p1);

			manager.onCellSelection(p1, 0, 0);
			manager.onCellSelection(p2, 0, 1);

			manager.onCellSelection(p1, 0, 2); //needs selection to move to move phase)
			Assertions.AssertThat(manager.phase()).IsEqual(BoardManager.GamePhase.MOVE);
			Assertions.AssertThat(manager.playerTurn()).IsEqual(p1);
			//did not complete movement, so should still be p1 turn

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

			manager.onCellSelection(p2, 0,0);
			Assertions.AssertThat(manager.phase()).IsEqual(BoardManager.GamePhase.MOVE);
			Assertions.AssertThat(manager.playerTurn()).IsEqual(p2);

		}

		[TestCase]
		public void placeToMoveTilePlacement(){
			manager.setTileCount(p1, 1);
			manager.setTileCount(p2, 1);
			Assertions.AssertThat(manager.phase()).IsEqual(BoardManager.GamePhase.PLACE);

			manager.onCellSelection(p1, 0, 0);
			Assertions.AssertThat(manager.board().tileAt(Location.at(0,0)).color()).IsEqual(p1);

			manager.onCellSelection(p2, 0, 1);
			Assertions.AssertThat(manager.board().tileAt(Location.at(0,1)).color()).IsEqual(p2);

			manager.onCellSelection(p1, 0, 2);
			Assertions.AssertThat(manager.phase()).IsEqual(BoardManager.GamePhase.MOVE);


			for (int i = 0; i < 5; i ++){
				for (int j = 0; j < 7; j++){
					Location currLoc = Location.at(i, j);
					Tile currTile = manager.board().tileAt(currLoc);
					if ((i == 0 && j < 2)){
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

		[TestCase]
		public async Task moveToWinScoutAndTilePass(){
			Tile winningTile = new Tile(p2);
			Scout winningScout = new Scout(p2);
			manager.board().addTile(winningTile, Location.at(3, 1));
			manager.board().addScout(winningScout, Location.at(3, 1));
			
			manager.setPhase(BoardManager.GamePhase.MOVE);
			manager.setTurn(p2);

			AssertSignal(_eventBus).StartMonitoring();
			AssertSignal(_eventBus).IsSignalExists("onPlayerWin");
			manager.onCellSelection(p2, 3, 1);
			manager.onCellSelection(p2, 3, 0);

			await AssertSignal(_eventBus).IsEmitted("onPlayerWin").WithTimeout(50);

			Assertions.AssertThat(manager.board().scoutAt(Location.at(3, 0))).IsNotNull();
		}

		[TestCase]
		public async Task moveToWinScoutKnockout(){
			for (int i = 0; i < 3; i++){
				Location currLoc = Location.at(1+i, 1);
				manager.board().addTile(new Tile(p2), currLoc);
			}
			manager.board().addScout(new Scout(p1), Location.at(0, 1));

			manager.board().scoutLayer()[2, 1] = null;

			AssertSignal(_eventBus).StartMonitoring();

			manager.setPhase(BoardManager.GamePhase.MOVE);
			manager.setTurn(p2);
			manager.onCellSelection(p2, 3, 1);
			manager.onCellSelection(p2, -1, 1);

			Assertions.AssertThat(manager.board().tileAt(Location.at(3,1))).IsNull();

			await AssertSignal(_eventBus).IsEmitted("onPlayerWin").WithTimeout(50);

		}

		[TestCase]
		public async Task moveToWinScoutKnockoutAndPass(){
			for (int i = 0; i < 3; i++){
				Location currLoc = Location.at(2, 2+i);
				manager.board().addTile(new Tile(p2), currLoc);
			}

			AssertSignal(_eventBus).StartMonitoring();

			manager.setPhase(BoardManager.GamePhase.MOVE);
			manager.setTurn(p2);
			manager.onCellSelection(p2, 2, 4);
			manager.onCellSelection(p2, 2, 0);
			manager.setTurn(p2);
			manager.onCellSelection(p2, 2, 3);
			manager.onCellSelection(p2, 2, -1);

			await AssertSignal(_eventBus).IsEmitted("onPlayerWin").WithTimeout(50);

		}

		[TestCase]
		public async Task moveToWinPushedIntoZone(){
			AssertSignal(_eventBus).StartMonitoring();

			for (int i = 0; i < 3; i++){
				Location currLoc = Location.at(3, 2+i);
				manager.board().addTile(new Tile(p1), currLoc);
			}
			manager.board().addScout(new Scout(p2), Location.at(3, 1));

			manager.setPhase(BoardManager.GamePhase.MOVE);
			manager.setTurn(p1);

			manager.onCellSelection(p1, 3, 4);
			manager.onCellSelection(p1, 3, 0);

			Assertions.AssertThat(manager.board().scoutAt(Location.at(3,0))).IsNotNull();

			await AssertSignal(_eventBus).IsEmitted("onPlayerWin").WithTimeout(50);
		}

	}
}
