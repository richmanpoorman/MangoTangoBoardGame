using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

// Manages the state of the board, and holds the actual board itself
public partial class BoardManager : Node
{ 

	public enum GamePhase {
		PLACE, MOVE 
	}

	// [Signal]
	// public delegate void SwitchPhaseEventHandler();
	// /* 
	// Input: Player color (current turn)
	// Input: Player color (current turn)
	// */
	// [Signal]
	// public delegate void SwitchPlayerEventHandler();
	// Set up
	private SteppingStonesBoard _board = new GridSteppingStonesBoard(5, 7); // new GridBoard(7, 5); 
	private Rules _ruleset = new ComposableRules(1, false, false); // new ComposableRules(1, false, false); 

	// private static int _totalTiles = 2;
	[Export] 
	private int player1DefaultTileCount = 20; 
	[Export]
	private int player2DefaultTileCount = 20; 

	private Dictionary<Piece.Color, int> tileCounts = new Dictionary<Piece.Color, int>() {
		{Piece.Color.PLAYER_1, 20}, 
		{Piece.Color.PLAYER_2, 20}
	};
	
	
	// Current board state
	private GamePhase gamePhase = GamePhase.PLACE;
	// private int tileCount = _totalTiles * 2;
	private Piece.Color currentPlayer = Piece.Color.PLAYER_1; 
	#nullable enable
	private Location? previousPosition = null; 
	#nullable disable

	

	// [Export]
	// private SelectSquare selector; 

	// [Export]
	// private BoardDisplay display; 

	[Export]
	private DisplayOptions moveOptionsDisplay; 
	private EventBus _eventBus; 

	public override void _Ready() {
		_eventBus = EventBus.Bus;
		_eventBus.onSelection += onCellSelection; 
		// Connect(EventBus.SignalName.onSelection, Callable.From(onCellSelection));
		onRestart();
	}

    /*board
    Inputs: None
    Returns: Board
    Description: returns the board BoardManager uses*/
	public Board board() { return _board; }

	/*onUpdate
    Inputs: None
    Returns: None
    Description: Updates display*/
    public void onUpdate() {
		// display.updateDisplay();
		_eventBus.EmitSignal(EventBus.SignalName.onBoardUpdate);
	}
	public Piece.Color playerTurn() { return currentPlayer;}
	public void setTurn(Piece.Color turn) { currentPlayer = turn;}
	public void setRules(Rules rules) { _ruleset = rules; }
	public void setBoard(SteppingStonesBoard board){ 
		_board = board; 
		onRestart();
	}
	public GamePhase phase() { return gamePhase; } 
	public void setPhase(GamePhase phase) {
		gamePhase = phase; 
		_eventBus.EmitSignal(EventBus.SignalName.onPhaseStart, (int)phase);
	}
	#nullable enable

	public void onWin() {
		GD.Print("Winner!");
		_eventBus.EmitSignal(EventBus.SignalName.onPlayerWin);
	}

	public void onRestart() {
		gamePhase = GamePhase.PLACE; 
		_eventBus.EmitSignal(EventBus.SignalName.onPhaseStart, (int)GamePhase.PLACE);
		currentPlayer = Piece.Color.PLAYER_1; 
		unmarkSelection(); 
		// tileCount = _totalTiles * 2;
		setTileCount(Piece.Color.PLAYER_1, player1DefaultTileCount);
		setTileCount(Piece.Color.PLAYER_2, player2DefaultTileCount);

		// display.initializeBoard(); 
		_eventBus.EmitSignal(EventBus.SignalName.onBoardReset);
		onUpdate();
	}

	private void switchPhases() {
		GD.Print("SWITCHING PHASE");
		switch(gamePhase) {
			case GamePhase.PLACE:
				gamePhase = GamePhase.MOVE; 
				_eventBus.EmitSignal(EventBus.SignalName.onPhaseStart, (int)GamePhase.MOVE);
			break; 
			case GamePhase.MOVE:
				gamePhase = GamePhase.MOVE;
			break;
		}
	}

	public int playerTileCount(Piece.Color player) {
		return tileCounts[player]; 
	} 

	public void setTileCount(Piece.Color player, int count) {
		tileCounts[player] = count; 
	}

	/*onSelection
	Inputs: None
	Returns: None
	Description: attempts selected action based on click; updates board; if action sucessful, switches player turn
	*/
	public void onCellSelection(Piece.Color player, int row, int column) {
		if (player != currentPlayer) return;  // If they play out of turn, don't let them 

		GD.Print("Manager sees player: ", player);

		Location selection = Location.at(row, column);
		// Location selection = selector.selection(); 

		// GD.Print("Selection: (", selection.row(), ", ", selection.column(), ")");

		// if (previousPosition is Location prev) 
		// 	GD.Print("Previous: (", prev.row(), ", ", prev.column(), ")");
		// else 	
		// 	GD.Print("Previous: null");	
		
		bool finishTurn = false;
		switch(gamePhase) {
			case GamePhase.PLACE: 
				finishTurn = duringPlacingPhase(selection); 
			break; 
			case GamePhase.MOVE:
				finishTurn = duringMovingPhase(selection);
			break; 
		}


		// switch(selector.mouseButton()) {
		// 	case MouseButton.Left:
		// 		finishTurn =  movePiece(selection);
		// 	break; 
		// 	case MouseButton.Right:
		// 		finishTurn = addTile(selection, currentPlayer);
		// 	break;
		// 	case MouseButton.Middle:
		// 		finishTurn = pushPieces(selection);
		// 	break; 
		// }
		

		onUpdate();

		if (finishTurn) {
			if (_ruleset.hasWon(_board, currentPlayer)) {
				onWin(); 
				return; 
			}
			switch(currentPlayer) {
				case Piece.Color.PLAYER_1:
				GD.Print("Switched to Player 2 from Player 1");
				currentPlayer = Piece.Color.PLAYER_2; 
				_eventBus.EmitSignal(EventBus.SignalName.onTurnChange, (int)Piece.Color.PLAYER_2);
				break; 
				case Piece.Color.PLAYER_2: 
				GD.Print("Switched to Player 1 from Player 2");
				currentPlayer = Piece.Color.PLAYER_1;
				_eventBus.EmitSignal(EventBus.SignalName.onTurnChange, (int)Piece.Color.PLAYER_1);
				break; 
			}
			if (_ruleset.hasWon(_board, currentPlayer)) { // Just in case opponent makes you win
				onWin(); 
				return; 
			}
		}
	}

	private bool duringPlacingPhase(Location selection) {
		
		if (tileCounts[currentPlayer] == 0) {
			switchPhases();
			return false; 
		} 
		bool didPlace = addTile(selection, currentPlayer);
		if (didPlace) tileCounts[currentPlayer] -= 1; 
		return didPlace;
	}

	private bool duringMovingPhase(Location selection) {
	
		if (previousPosition == null) {
			markSelection(selection); 
			return false; 
		}
		bool isAdjacentCell = (Math.Abs(selection.row() - previousPosition.row()) + Math.Abs(selection.column() - previousPosition.column())) == 1;
		if (isAdjacentCell) return movePiece(selection);
		else return pushPieces(selection);
	}

	#nullable disable

	/*pushPieces
	Inputs: Location
	Returns: bool
	Description: Attempts to push pieces from previous selection to Location;
	returns sucess of attempt;
	*/
	private bool pushPieces(Location selection) {
		if (previousPosition == null) {
			markSelection(selection);
			return false;
		}
		GD.Print("Attempt Push");
		if (!_ruleset.isValidPush(_board, previousPosition, selection, currentPlayer)) {
			unmarkSelection();
			return false; 
		}
		bool isSuccess = _board.pushMove(previousPosition, selection);
		unmarkSelection(); 
		if (isSuccess) _eventBus.EmitSignal(EventBus.SignalName.onTilePush); 
		return isSuccess; 
	}	
	/*addTile
	Inputs: Location, Piece.Color
	Returns: bool
	Description: Attempts to add tile of given color at location; returns sucess of attempt*/
	private bool addTile(Location selection, Piece.Color color) {
		if (!_board.isOnBoard(selection))
			return false;
		if (!_ruleset.isValidPlace(_board, selection, color)) return false;
		GD.Print("Attempt Add");
		bool isSuccess = _board.placeTile(new Tile(color), selection);
		unmarkSelection(); 
		if (isSuccess) _eventBus.EmitSignal(EventBus.SignalName.onTilePlace); 
		return isSuccess;
		// _board.addTile(new Tile(color), selection);
		// return true;
	}

	#nullable enable
	
	/* movePiece
	Inputs: Location
	Returns: bool
	Description: attempts to move tile from previous selected position to current selected position;
	returns sucess of attempt*/
	private bool movePiece(Location selection) {
		if (previousPosition == null) {
			markSelection(selection); 
			return false;
		}
		GD.Print("Attempt Move");
		if (!_ruleset.isValidPieceMove(_board, previousPosition, selection, currentPlayer)) { 
			unmarkSelection(); 
			return false; 
		}
		bool isTileMove = _board.tileAt(selection) == null; 
		bool isSuccess = _board.movePiece(previousPosition, selection);
		unmarkSelection();
		if (isSuccess) {
			string signal = isTileMove ? EventBus.SignalName.onTileMove : EventBus.SignalName.onScoutMove; 
			_eventBus.EmitSignal(signal); 
		}
		return isSuccess;
	}

 	/*giveValidPreviousSelection
	Inputs: Location selection
	Returns: Location or null
	Description: If selection is on the board and there is a tile at that location, returns selection; else null*/
	private Location? giveValidPreviousSelection(Location selection) {
		int[] size = _board.size(); 
		if (selection.row() < 0 || selection.row() >= size[0] || selection.column() < 0 || selection.column() >= size[1])
			return null; 
		if (_board.tileAt(selection) == null) 
			return null;
		return selection;
	}

	private void markSelection(Location selection) {
		previousPosition = giveValidPreviousSelection(selection); 
		if (previousPosition == null) return;
		IList<Rules.ValidMove> options = _ruleset.legalOptions(_board, previousPosition, currentPlayer);  
		moveOptionsDisplay.updateOptions(options);
	}

	private void unmarkSelection() {
		previousPosition = null; 
		moveOptionsDisplay.clear(); 
	}
	#nullable disable
}
