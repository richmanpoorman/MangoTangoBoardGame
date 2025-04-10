// GdUnit generated TestSuite
using Godot;
using GdUnit4;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GdUnitDefaultTestNamespace
{
	using static Assertions;
	using static Utils;

	[TestSuite]
	public class Timing_test
	{
		private ISceneRunner runner;
		private BoardManager manager;
		private GridSteppingStonesBoard board;
        Piece.Color p1, p2;
        private Stopwatch stopwatch;
        private const int maxMoveMilli = 100;
        private const int maxSaveLoadMilli = 1000;

		[Before]
		public void setup(){
            stopwatch = new Stopwatch();
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
		public void timePlaceTile()
		{
            Location loc = Location.at(0,0);
            manager.setTurn(p1);
            stopwatch.Start();
            manager.onCellSelection(p1, loc.row(), loc.column());
            stopwatch.Stop();
            Assertions.AssertThat(manager.board().tileAt(loc).color()).IsEqual(p1);
            Assertions.AssertThat(stopwatch.ElapsedMilliseconds).IsLessEqual(maxMoveMilli);
			
		}

        [TestCase]
        public void timeMoveTile(){
            Location loc = Location.at(0,0);
            Location loc2 = Location.at(1, 0);
            manager.setTurn(p1);
            manager.onCellSelection(p1, loc.row(), loc.column());
            manager.setPhase(BoardManager.GamePhase.MOVE);
            manager.setTurn(p1);

            stopwatch.Start();
            manager.onCellSelection(p1, loc.row(), loc.column());
            manager.onCellSelection(p1, loc2.row(), loc2.column());
            stopwatch.Stop();

            Assertions.AssertThat(manager.board().tileAt(loc2).color()).IsEqual(p1);
            Assertions.AssertThat(manager.board().tileAt(loc)).IsNull();
            Assertions.AssertThat(stopwatch.ElapsedMilliseconds).IsLessEqual(maxMoveMilli);
        }

        [TestCase]
        public void timePush(){
            Location loc0 = Location.at(0,4);
            Location loc1 = Location.at(3, 4); //tile to push to
            manager.setTurn(p1);
            manager.onCellSelection(p1, loc0.row(), loc0.column());
            manager.onCellSelection(p2, 2, 4);
            manager.onCellSelection(p1, 1, 4);
            // [1][1][2] form;
            manager.setPhase(BoardManager.GamePhase.MOVE);
            manager.setTurn(p1);

            stopwatch.Start();
            manager.onCellSelection(p1, loc0.row(), loc0.column());
            manager.onCellSelection(p1, loc1.row(), loc1.column());
            stopwatch.Stop();

            Assertions.AssertThat(manager.board().tileAt(loc1).color()).IsEqual(p2);
            Assertions.AssertThat(manager.board().tileAt(loc0)).IsNull();
            Assertions.AssertThat(stopwatch.ElapsedMilliseconds).IsLessEqual(maxMoveMilli);
        }

        [TestCase]
        public void timeMoveScout(){
            Location loc = Location.at(5/2, 2); //next to red scout
            Location scoutStart = Location.at(2, 1);
            manager.setTurn(p1);
            manager.onCellSelection(p1, loc.row(), loc.column());
            manager.setPhase(BoardManager.GamePhase.MOVE);
            manager.setTurn(p1);

            stopwatch.Start();
            manager.onCellSelection(p1, scoutStart.row(), scoutStart.column());
            manager.onCellSelection(p1, loc.row(), loc.column());
            stopwatch.Stop();

            Assertions.AssertThat(manager.board().scoutAt(loc).color()).IsEqual(p1);
            Assertions.AssertThat(manager.board().scoutAt(scoutStart)).IsNull();
            Assertions.AssertThat(stopwatch.ElapsedMilliseconds).IsLessEqual(maxMoveMilli);

        }
        
        [TestCase]
        public void timeMoveScoutAndTile(){
            Location loc = Location.at(5/2, 2); //next to red scout
            Location scoutStart = Location.at(2, 1);
            manager.setPhase(BoardManager.GamePhase.MOVE);
            manager.setTurn(p1);

            stopwatch.Start();
            manager.onCellSelection(p1, scoutStart.row(), scoutStart.column());
            manager.onCellSelection(p1, loc.row(), loc.column());
            stopwatch.Stop();

            Assertions.AssertThat(manager.board().scoutAt(loc).color()).IsEqual(p1);
            Assertions.AssertThat(manager.board().scoutAt(scoutStart)).IsNull();
            Assertions.AssertThat(stopwatch.ElapsedMilliseconds).IsLessEqual(maxMoveMilli);
        }

        [TestCase]
        public void timeLoad(){
            GameSaver saver = new GameSaver();
		    string testFile = "res://test/pushTest_space.step";
            stopwatch.Start();
		    (SteppingStonesBoard lboard, Piece.Color turn1, int p1Tiles, 
			    int p2Tiles, BoardManager.GamePhase phase) = saver.LoadGame(testFile);
            stopwatch.Stop();

            Assertions.AssertThat(stopwatch.ElapsedMilliseconds).IsLessEqual(maxSaveLoadMilli);
        }


        [TestCase]
        public void timeSave(){
            GameSaver saver = new GameSaver();

            SteppingStonesBoard board = new GridSteppingStonesBoard(5, 7);
            stopwatch.Start();
		    saver.SaveGame(board, Piece.Color.PLAYER_1, 3, 3,
		    BoardManager.GamePhase.PLACE, "user://savertest.step");
            stopwatch.Stop();
            Assertions.AssertThat(stopwatch.ElapsedMilliseconds).IsLessEqual(maxSaveLoadMilli);
        }

		

	}
}
