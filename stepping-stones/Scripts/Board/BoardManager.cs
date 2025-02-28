using Godot;
using System;
using System.Collections.Generic;
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
	private Rules _ruleset = new WeightedScout(); 

	private static int _totalTiles = 2;
	
	
	// Current board state
	private GamePhase gamePhase = GamePhase.PLACE;
	private int tileCount = _totalTiles * 2;
	private Piece.Color currentPlayer = Piece.Color.PLAYER_1; 
	#nullable enable
	private Location? previousPosition = null; 
	#nullable disable

	[Export]
	private SelectSquare selector; 

	[Export]
	private BoardDisplay display; 

	[Export]
	private DisplayOptions moveOptionsDisplay; 

	[Signal]
	public delegate void onPlayerWinEventHandler(); 

	// Signals to listen for 

	[Signal]
	public delegate void onBoardResetEventHandler(); 

	[Signal]
	public delegate void onTurnChangeEventHandler(Piece.Color turn); 

	[Signal]
	public delegate void onMovementPhaseStartEventHandler(); 

	[Signal]
	public delegate void onPhaseStartEventHandler(GamePhase phase); 

	[Signal]
	public delegate void onBoardUpdateEventHandler();

	[Signal]
	public delegate void onTilePlaceEventHandler(); 

	[Signal]
	public delegate void onScoutMoveEventHandler(); 

	[Signal]
	public delegate void onTileMoveEventHandler(); 

	[Signal]
	public delegate void onTilePushEventHandler(); 


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
		display.updateDisplay();
		EmitSignal(SignalName.onBoardUpdate);
	}
	public Piece.Color playerTurn() { return currentPlayer;}
	public void setTurn(Piece.Color turn) { currentPlayer = turn;}
	public void setBoard(SteppingStonesBoard board){ 
		_board = board; 
		onRestart();
	}
	
	#nullable enable

	public void onWin() {
		GD.Print("Winner!");
		EmitSignal(SignalName.onPlayerWin);
	}

	public void onRestart() {
		gamePhase = GamePhase.PLACE; 
		EmitSignal(SignalName.onPhaseStart, (int)GamePhase.PLACE);
		currentPlayer = Piece.Color.PLAYER_1; 
		unmarkSelection(); 
		tileCount = _totalTiles * 2;


		display.initializeBoard(); 
		EmitSignal(SignalName.onBoardReset);
		onUpdate();
	}

	private void switchPhases() {
		GD.Print("SWITCHING PHASE");
		switch(gamePhase) {
			case GamePhase.PLACE:
				gamePhase = GamePhase.MOVE; 
				EmitSignal(SignalName.onPhaseStart, (int)GamePhase.MOVE);
			break; 
			case GamePhase.MOVE:
				gamePhase = GamePhase.MOVE;
			break;
		}
	}

	/*onSelection
	Inputs: None
	Returns: None
	Description: attempts selected action based on click; updates board; if action sucessful, switches player turn
	*/
	public void onSelection(int row, int column) {
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
				EmitSignal(SignalName.onTurnChange, (int)Piece.Color.PLAYER_2);
				break; 
				case Piece.Color.PLAYER_2: 
				GD.Print("Switched to Player 1 from Player 2");
				currentPlayer = Piece.Color.PLAYER_1;
				EmitSignal(SignalName.onTurnChange, (int)Piece.Color.PLAYER_1);
				break; 
			}
			if (_ruleset.hasWon(_board, currentPlayer)) { // Just in case opponent makes you win
				onWin(); 
				return; 
			}
		}
	}

	private bool duringPlacingPhase(Location selection) {
		
		bool didPlace = addTile(selection, currentPlayer);
		if (didPlace) tileCount -= 1; 
		if (tileCount == 0) switchPhases(); 
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
		if (isSuccess) EmitSignal(SignalName.onTilePush); 
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
		if (isSuccess) EmitSignal(SignalName.onTilePlace); 
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
		bool isSuccess = _board.movePiece(previousPosition, selection);
		unmarkSelection();
		if (isSuccess) EmitSignal(SignalName.onTileMove);
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
