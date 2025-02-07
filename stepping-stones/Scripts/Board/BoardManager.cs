using Godot;
using System;
using System.Collections.Generic;

// Manages the state of the board, and holds the actual board itself
public partial class BoardManager : Node
{ 
	private SteppingStonesBoard _board = new GridSteppingStonesBoard(7, 5); // new GridBoard(7, 5); 
	private Rules _ruleset = new BasicRules(); 
	
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

    /*setBoard
    Inputs: SteppingStonesBoard board
    Returns: None
    Description: Sets internal board to given board*/
	public void setBoard(SteppingStonesBoard board){ _board = board; }
	
	/*onSelection
	Inputs: None
	Returns: None
	Description: attempts selected action based on click; updates board; if action sucessful, switches player turn
	*/
	#nullable enable
	public void onSelection() {
		Location selection = selector.selection(); 

		GD.Print("Selection: (", selection.row(), ", ", selection.column(), ")");

		if (previousPosition is Location prev) 
			GD.Print("Previous: (", prev.row(), ", ", prev.column(), ")");
		else 	
			GD.Print("Previous: null");	
		
		bool finishTurn = false;
		switch(selector.mouseButton()) {
			case MouseButton.Left:
				finishTurn =  movePiece(selection);
			break; 
			case MouseButton.Right:
				finishTurn = addTile(selection, currentPlayer);
			break;
			case MouseButton.Middle:
				finishTurn = pushPieces(selection);
			break; 
		}
		

		onUpdate();

		if (finishTurn) {
			
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
