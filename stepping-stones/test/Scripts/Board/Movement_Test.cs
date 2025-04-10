// GdUnit generated TestSuite
using Godot;
using GdUnit4;
using System.Threading.Tasks;

namespace GdUnitDefaultTestNamespace
{
	using static Assertions;
	using static Utils;

	[TestSuite]
	public class Movement_Test
	{
		private ISceneRunner runner;
		private BoardManager manager;
		private GridSteppingStonesBoard board;
		[Before]
		public void setup(){
			runner = ISceneRunner.Load("res://Scenes/Board_Scene.tscn");
			manager = (BoardManager)runner.FindChild("BoardManager");
			board = new GridSteppingStonesBoard(5, 7);
			manager.setBoard(board);

		}
		// TestSuite generated from
		private const string sourceClazzPath = "./../../../Scripts/UILogic/MainGame.cs";
		[TestCase]
		public void setPlayerTile()
		{
			// ISceneRunner runner = ISceneRunner.Load("res://Scenes/Board_Scene.tscn");
			// BoardManager manager = (BoardManager)runner.FindChild("BoardManager");//game.manager;
			// GridSteppingStonesBoard board = new GridSteppingStonesBoard(5, 7);
			// manager.setBoard(board);
			Tile tile = new Tile(Piece.Color.PLAYER_1);
			board.placeTile(tile, Location.at(2, 3));
			manager.setTileCount(Piece.Color.PLAYER_1, 5);
			Assertions.AssertThat(manager.playerTileCount(Piece.Color.PLAYER_1)).IsEqual(5);
			manager.setTileCount(Piece.Color.PLAYER_2, 6);
			Assertions.AssertThat(manager.playerTileCount(Piece.Color.PLAYER_2)).IsEqual(6);
		}

		[TestCase]
		public void turnColorPlaceTestP1(){
			// ISceneRunner runner = ISceneRunner.Load("res://Scenes/Board_Scene.tscn");
			// BoardManager manager = (BoardManager)runner.FindChild("BoardManager");//game.manager;
			// GridSteppingStonesBoard board = new GridSteppingStonesBoard(5, 7);
			// manager.setBoard(board);
			
			Piece.Color turn1 = manager.playerTurn();
			Assertions.AssertThat(manager.board().tileAt(Location.at(1,2))).IsEqual(null);
			manager.onCellSelection(turn1, 1, 2);
			Assertions.AssertThat(manager.board().tileAt(Location.at(1,2)).color()).IsEqual(turn1);
			Assertions.AssertThat(manager.playerTurn()).IsNotEqual(turn1);

			//check that nothing happens when p1 tries to go on p2 turn
			Assertions.AssertThat(manager.board().tileAt(Location.at(2,2))).IsEqual(null);
			manager.onCellSelection(turn1, 2, 2);
			Assertions.AssertThat(manager.board().tileAt(Location.at(2,2))).IsEqual(null);
			Assertions.AssertThat(manager.playerTurn()).IsNotEqual(turn1);

		}

		[TestCase]
		public void boundPlaceTest(){
			// ISceneRunner runner = ISceneRunner.Load("res://Scenes/Board_Scene.tscn");
			// BoardManager manager = (BoardManager)runner.FindChild("BoardManager");//game.manager;
			// GridSteppingStonesBoard board = new GridSteppingStonesBoard(5, 7);
			// manager.setBoard(board);

			Piece.Color turn1 = manager.playerTurn();
			int numTiles = manager.playerTileCount(turn1);
			manager.onCellSelection(turn1, -1, 2);
			//make sure that trying to place in invalid spot did nothing; since
			//doesn't exist on board, can't make sure that that spot is still null
			Assertions.AssertThat(manager.playerTurn()).IsEqual(turn1);
			Assertions.AssertThat(manager.playerTileCount(turn1)).IsEqual(numTiles);

		}

	[TestCase]
	public void noMoveOutOfBounds(){
		// ISceneRunner runner = ISceneRunner.Load("res://Scenes/Board_Scene.tscn");
		// BoardManager manager = (BoardManager)runner.FindChild("BoardManager");//game.manager;
		// GridSteppingStonesBoard board = new GridSteppingStonesBoard(5, 7);
		// manager.setBoard(board);

		Piece.Color turn1 = manager.playerTurn();
		manager.onCellSelection(turn1, 0, 0);
		Assertions.AssertThat(manager.board().tileAt(Location.at(0,0)).color()).IsEqual(turn1);
		manager.setPhase(BoardManager.GamePhase.MOVE);
		manager.setTurn(turn1);
		
		manager.onCellSelection(turn1, 0, 0);
		manager.onCellSelection(turn1, -1, 0);
		Assertions.AssertThat(manager.board().tileAt(Location.at(0,0)).color()).IsEqual(turn1);
		Assertions.AssertThat(manager.playerTurn()).IsEqual(turn1);

		manager.onCellSelection(turn1, 0, 0);
		manager.onCellSelection(turn1, 0, -1);
		Assertions.AssertThat(manager.board().tileAt(Location.at(0,0)).color()).IsEqual(turn1);
		Assertions.AssertThat(manager.playerTurn()).IsEqual(turn1);

		//Tests going negative: TODO: Potential extension: overshoot?

	}

	[TestCase]
	public async Task noMoveWrongColor(){
		// Ran into issue where this was failing turn order when using geral runner & manager?

		ISceneRunner runner2 = ISceneRunner.Load("res://Scenes/Board_Scene.tscn");
		BoardManager manager2 = (BoardManager)runner2.FindChild("BoardManager");
		GridSteppingStonesBoard board2 = new GridSteppingStonesBoard(5, 7);
		manager2.setBoard(board2);

		Piece.Color turn1 = Piece.Color.PLAYER_1;
		Piece.Color turn2 = Piece.Color.PLAYER_2;
		manager2.setTurn(turn1);
		await runner2.SimulateFrames(10);
		Assertions.AssertThat(manager2.playerTurn()).IsEqual(turn1);

		manager2.onCellSelection(turn1, 0, 0);
		await runner2.SimulateFrames(10);
		Assertions.AssertThat(manager2.board().tileAt(Location.at(0,0)).color()).IsEqual(turn1);

		manager2.setPhase(BoardManager.GamePhase.MOVE);
		
		manager2.setTurn(turn2);
		Assertions.AssertThat(manager2.playerTurn()).IsNotEqual(turn1);
		Assertions.AssertThat(manager2.playerTurn()).IsEqual(turn2);

		//Is turn2's turn, but is trying to move turn1 color
		manager2.onCellSelection(turn2, 0, 0);
		manager2.onCellSelection(turn2, 1, 0);
		Assertions.AssertThat(manager2.board().tileAt(Location.at(0,0)).color()).IsEqual(turn1);
		Assertions.AssertThat(manager2.board().tileAt(Location.at(1,0))).IsNull();

		//Extention: more directions, both players

	}

	[TestCase]
	public void scoutlessPushRight(){
		GameSaver saver = new GameSaver();
		string testFile = "res://test/pushTest_space.step";
		(SteppingStonesBoard lboard, Piece.Color turn1, int p1Tiles, 
			int p2Tiles, BoardManager.GamePhase phase) = saver.LoadGame(testFile);
		Assertions.AssertThat(phase).IsEqual(BoardManager.GamePhase.MOVE);
		manager.setBoard(lboard);
		manager.setPhase(phase);
		manager.setTurn(turn1);

		Assertions.AssertThat( manager.board().tileAt(Location.at(2,1)).color()).IsEqual(turn1);
		
		manager.onCellSelection(turn1, 2, 1);
		manager.onCellSelection(turn1, 2, 4);
		
		Assertions.AssertThat(manager.board().tileAt(Location.at(2,1))).IsNull();
		Assertions.AssertThat(manager.board().tileAt(Location.at(2,2)).color()).IsEqual(turn1);
		Assertions.AssertThat(manager.board().tileAt(Location.at(2,3)).color()).IsEqual(turn1);
		Assertions.AssertThat(manager.board().tileAt(Location.at(2,4)).color()).IsEqual(manager.playerTurn());

		Assertions.AssertThat(manager.playerTurn()).IsNotEqual(turn1);


	}

	[TestCase]
	public void scoutlessPushLeft(){
		GameSaver saver = new GameSaver();
		string testFile = "res://test/pushTest_space.step";
		(SteppingStonesBoard lboard, Piece.Color turn1, int p1Tiles, 
			int p2Tiles, BoardManager.GamePhase phase) = saver.LoadGame(testFile);
		Assertions.AssertThat(phase).IsEqual(BoardManager.GamePhase.MOVE);
		manager.setBoard(lboard);
		manager.setPhase(phase);
		manager.setTurn(turn1);

		Assertions.AssertThat( manager.board().tileAt(Location.at(2,1)).color()).IsEqual(turn1);
		
		manager.onCellSelection(turn1, 6, 5);
		manager.onCellSelection(turn1, 6, 2);
		
		Assertions.AssertThat(manager.board().tileAt(Location.at(6,5))).IsNull();
		Assertions.AssertThat(manager.board().tileAt(Location.at(6,4)).color()).IsEqual(turn1);
		Assertions.AssertThat(manager.board().tileAt(Location.at(6,3)).color()).IsEqual(turn1);
		Assertions.AssertThat(manager.board().tileAt(Location.at(6,2)).color()).IsEqual(manager.playerTurn());

		Assertions.AssertThat(manager.playerTurn()).IsNotEqual(turn1);



	}


	[TestCase]
	public void scoutlessPushUp(){
		
		GameSaver saver = new GameSaver();
		string testFile = "res://test/pushTest_space.step";
		(SteppingStonesBoard lboard, Piece.Color turn1, int p1Tiles, 
			int p2Tiles, BoardManager.GamePhase phase) = saver.LoadGame(testFile);
		Assertions.AssertThat(phase).IsEqual(BoardManager.GamePhase.MOVE);
		manager.setBoard(lboard);
		manager.setPhase(phase);
		
		turn1 = Piece.Color.PLAYER_1;
		Piece.Color turn2 = Piece.Color.PLAYER_2;
		manager.setTurn(turn2);
		
		manager.onCellSelection(turn2, 4, 2);
		manager.onCellSelection(turn2, 1, 2);
		
		Assertions.AssertThat(manager.board().tileAt(Location.at(4,2))).IsNull();
		Assertions.AssertThat(manager.board().tileAt(Location.at(3,2)).color()).IsEqual(turn2);
		Assertions.AssertThat(manager.board().tileAt(Location.at(2,2)).color()).IsEqual(turn2);
		Assertions.AssertThat(manager.board().tileAt(Location.at(1,2)).color()).IsEqual(manager.playerTurn());

		Assertions.AssertThat(manager.playerTurn()).IsNotEqual(turn2);

	}


	[TestCase]
	public void scoutlessPushDown(){
		GameSaver saver = new GameSaver();
		string testFile = "res://test/pushTest_space.step";
		(SteppingStonesBoard lboard, Piece.Color turn1, int p1Tiles, 
			int p2Tiles, BoardManager.GamePhase phase) = saver.LoadGame(testFile);
		Assertions.AssertThat(phase).IsEqual(BoardManager.GamePhase.MOVE);
		manager.setBoard(lboard);
		manager.setPhase(phase);
		manager.setTurn(turn1);

		Assertions.AssertThat( manager.board().tileAt(Location.at(2,1)).color()).IsEqual(turn1);
		
		manager.onCellSelection(turn1, 0, 3);
		manager.onCellSelection(turn1, 3, 3);
		
		Assertions.AssertThat(manager.board().tileAt(Location.at(0,3))).IsNull();
		Assertions.AssertThat(manager.board().tileAt(Location.at(1,3)).color()).IsEqual(turn1);
		Assertions.AssertThat(manager.board().tileAt(Location.at(2,3)).color()).IsEqual(turn1);
		Assertions.AssertThat(manager.board().tileAt(Location.at(3,3)).color()).IsEqual(manager.playerTurn());

		Assertions.AssertThat(manager.playerTurn()).IsNotEqual(turn1);
	}


	// 	public void blockWrongScoutlessPushRight(){}
	// 	public void blockWrongScoutlessPushLeft(){}
	// 	public void blockWrongScoutlessPushUp(){}
	// 	public void blockWrongScoutlessPushDown(){}

	[TestCase]
	public void scoutMoveToOwn(){
		//red scout at (2, 1) on 5x7 board
		Piece.Color p1 = Piece.Color.PLAYER_1;
		AssertThat(manager.board().scoutAt(Location.at(2, 1))).IsNotEqual(null);
		manager.setTurn(p1);
		manager.onCellSelection(p1, 3, 1);
		manager.setPhase(BoardManager.GamePhase.MOVE);
		manager.setTurn(p1);
		manager.onCellSelection(p1, 2, 1);
		manager.onCellSelection(p1, 3, 1);

		AssertThat(manager.board().scoutAt(Location.at(2,1))).IsNull();
		AssertThat(manager.board().scoutAt(Location.at(3,1)).color()).IsEqual(p1);
		AssertThat(manager.playerTurn()).IsNotEqual(p1);
	}

	[TestCase]
	public void tileMoveOnScoutToEmpty(){
		Piece.Color p1 = Piece.Color.PLAYER_1;
		AssertThat(manager.board().scoutAt(Location.at(2, 1)).color()).IsEqual(p1);
		manager.setPhase(BoardManager.GamePhase.MOVE);
		Location loc1 = Location.at(2, 1);
		Location loc2 = Location.at(3, 1);
		manager.setTurn(p1);
		manager.onCellSelection(p1, loc1.row(), loc1.column());
		manager.onCellSelection(p1, loc2.row(), loc2.column());

		//TODO: FIX
		AssertThat(manager.board().scoutAt(loc1)).IsNull();
		AssertThat(manager.board().scoutAt(loc2).color()).IsEqual(p1);
		AssertThat(manager.board().tileAt(loc1)).IsNull();
		AssertThat(manager.board().tileAt(loc2).color()).IsEqual(p1);
		AssertThat(manager.playerTurn()).IsNotEqual(p1);
	}

	[TestCase]
	public void blockMoveScoutToOther(){
		Piece.Color p1 = Piece.Color.PLAYER_1;
		Piece.Color p2 = Piece.Color.PLAYER_2;
		AssertThat(manager.board().scoutAt(Location.at(2, 1)).color()).IsEqual(p1);
		manager.setTurn(p2);
		manager.onCellSelection(p2, 3, 1);
		manager.onCellSelection(p1, 4, 1);
		manager.setPhase(BoardManager.GamePhase.MOVE);
		manager.setTurn(p1);
		manager.onCellSelection(p1, 2, 1);
		manager.onCellSelection(p1, 3, 1);

		AssertThat(manager.board().scoutAt(Location.at(2,1)).color()).IsEqual(p1);
		AssertThat(manager.board().scoutAt(Location.at(3,1))).IsNull();
		//Invalid move, so turn should remain the same.
		AssertThat(manager.playerTurn()).IsEqual(p1);
	}

	}
}
