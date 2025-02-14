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
	}
	public void setBoard(SteppingStonesBoard board){ 
		_board = board; 
		onRestart();
	}
	
	#nullable enable

	public void onWin() {
		GD.Print("Winner!");
	}

	public void onRestart() {
		gamePhase = GamePhase.PLACE; 
		currentPlayer = Piece.Color.PLAYER_1; 
		unmarkSelection(); 
		tileCount = _totalTiles * 2;


		display.initializeBoard(); 
		
		onUpdate();
	}

	private void switchPhases() {
		GD.Print("SWITCHING PHASE");
		switch(gamePhase) {
			case GamePhase.PLACE:
				gamePhase = GamePhase.MOVE; 
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
	
	public void onSelection() {
		// Location selection = selector.selection(); 

		// GD.Print("Selection: (", selection.row(), ", ", selection.column(), ")");

		// if (previousPosition is Location prev) 
		// 	GD.Print("Previous: (", prev.row(), ", ", prev.column(), ")");
		// else 	
		// 	GD.Print("Previous: null");	
		
		bool finishTurn = false;
		switch(gamePhase) {
			case GamePhase.PLACE: 
				finishTurn = duringPlacingPhase(); 
			break; 
			case GamePhase.MOVE:
				finishTurn = duringMovingPhase();
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
				break; 
				case Piece.Color.PLAYER_2: 
				GD.Print("Switched to Player 1 from Player 2");
				currentPlayer = Piece.Color.PLAYER_1;
				break; 
			}
			if (_ruleset.hasWon(_board, currentPlayer)) { // Just in case opponent makes you win
				onWin(); 
				return; 
			}
		}
	}

	private bool duringPlacingPhase() {
		
		Location selection = selector.selection(); 
		switch(selector.mouseButton()) {
			case MouseButton.Left:
				bool didPlace = addTile(selection, currentPlayer);
				if (didPlace) tileCount -= 1; 
				if (tileCount == 0) switchPhases(); 
				return didPlace;
			default:
				return false;
		}
	}

	private bool duringMovingPhase() {
		
		Location selection = selector.selection(); 
		switch(selector.mouseButton()) {
			case MouseButton.Left:
				if (previousPosition == null) {
					markSelection(selection); 
					return false; 
				}
				bool isAdjacentCell = (Math.Abs(selection.row() - previousPosition.row()) + Math.Abs(selection.column() - previousPosition.column())) == 1;
				if (isAdjacentCell) return movePiece(selection);
				else return pushPieces(selection);
			case MouseButton.Right: 
				unmarkSelection();
				return false;
			default:
				return false;
		}
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
