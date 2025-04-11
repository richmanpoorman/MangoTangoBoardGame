// GdUnit generated TestSuite
using Godot;
using GdUnit4;
using System.Threading.Tasks;
using GodotPlugins.Game;
using System.Threading;

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



		[Before]
		public void setup(){
			// runner = ISceneRunner.Load("res://Scenes/Board_Scene.tscn");
			// manager = (BoardManager)runner.FindChild("BoardManager");
			// board = new GridSteppingStonesBoard(5, 7);
			// manager.setBoard(board);
			p1 = Piece.Color.PLAYER_1;
            p2 = Piece.Color.PLAYER_2;

		}
		// // TestSuite generated from
		// private const string sourceClazzPath = "./../../../Scripts/UILogic/MainGame.cs";
		
		// [TestCase]
		// public void placeToMoveIsAutomatic(){
		// 	manager.setTileCount(p1, 1);
		// 	manager.setTileCount(p2, 1);
		// 	Assertions.AssertThat(manager.phase()).IsEqual(BoardManager.GamePhase.PLACE);

		// 	manager.onCellSelection(p1, 0, 0);
		// 	manager.onCellSelection(p2, 0, 1);
		// 	Assertions.AssertThat(manager.phase()).IsEqual(BoardManager.GamePhase.MOVE);


		// }

		// [TestCase]
		// public void placeToMoveTurnOrderP1()
		// {
		// 	manager.setTileCount(p1, 1);
		// 	manager.setTileCount(p2, 1);
		// 	Assertions.AssertThat(manager.phase()).IsEqual(BoardManager.GamePhase.PLACE);
		// 	Assertions.AssertThat(manager.playerTurn()).IsEqual(p1);

		// 	manager.onCellSelection(p1, 0, 0);
		// 	manager.onCellSelection(p2, 0, 1);
		// 	Assertions.AssertThat(manager.phase()).IsEqual(BoardManager.GamePhase.MOVE);
		// 	Assertions.AssertThat(manager.playerTurn()).IsEqual(p1);

		// }

		[TestCase]
		public async void move()
		{
			SceneManager sm = SceneManager.Instance;
			EventBus _eventBus = EventBus.Bus;
			sm.p1Tiles = 4;
			sm.p2Tiles = 4;
			sm.newGame = false;
			sm.board = new GridSteppingStonesBoard(5, 7);
			sm.phase = BoardManager.GamePhase.MOVE;
			runner = ISceneRunner.Load("res://Scenes/Main.tscn");
		
			// GD.Print("printing works")
			await runner.AwaitIdleFrame();
			_eventBus.EmitSignal(EventBus.SignalName.onSelection, (int)p1, 2, 1);
			var watch = System.Diagnostics.Stopwatch.StartNew();
			_eventBus.EmitSignal(EventBus.SignalName.onSelection, (int)p1, 2, 2);
			await runner.AwaitSignal("onScoutMove");
			watch.Stop();
			GD.Print($"Move took {watch.ElapsedMilliseconds} Ms");

		}

		[TestCase]
		public async Task restartToPlaceNoMove(){
			SceneManager sm = SceneManager.Instance;
			sm.p1Tiles = 4;
			sm.p2Tiles = 4;
			sm.newGame = false;
			sm.board = new GridSteppingStonesBoard(5, 7);
			sm.phase = BoardManager.GamePhase.MOVE;
			runner = ISceneRunner.Load("res://Scenes/Main.tscn");
		
			// GD.Print("printing works")
			await runner.AwaitIdleFrame();
			manager = runner.GetProperty<BoardManager>("manager");
			BoardManager.GamePhase phase = (BoardManager.GamePhase)runner.GetProperty("phase");
			GD.Print($"phase is {phase}");
			if (manager is null) {
				GD.Print("Null manager");
			}
			// runner.Invoke("_Ready");
			AssertThat(phase).IsEqual(BoardManager.GamePhase.MOVE);
			runner.Invoke("OnUIResetGame");
			AssertThat(manager.phase()).IsEqual(BoardManager.GamePhase.PLACE);
		}
		[TestCase]
		public async Task pushOffBoard(){
			SceneManager sm = SceneManager.Instance;
			sm.p1Tiles = 4;
			sm.p2Tiles = 4;
			sm.newGame = false;
			sm.board = new GridSteppingStonesBoard(5, 7);
			sm.phase = BoardManager.GamePhase.MOVE;
			runner = ISceneRunner.Load("res://Scenes/Main.tscn");
		
			// GD.Print("printing works")
			await runner.AwaitIdleFrame();
			manager = runner.GetProperty<BoardManager>("manager");
			BoardManager.GamePhase phase = (BoardManager.GamePhase)runner.GetProperty("phase");
			GD.Print($"phase is {phase}");
			if (manager is null) {
				GD.Print("Null manager");
			}
			// runner.Invoke("_Ready");
			AssertThat(phase).IsEqual(BoardManager.GamePhase.MOVE);
			runner.Invoke("OnUIResetGame");
			AssertThat(manager.phase()).IsEqual(BoardManager.GamePhase.PLACE);
		}


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