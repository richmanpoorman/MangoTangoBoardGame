using Godot;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

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

	public Board board() { return _board; }

    public void onUpdate() {
		display.updateDisplay();
	}

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

	private bool pushPieces(Location selection) {
		if (previousPosition == null) {
			previousPosition = giveValidPreviousSelection(selection);
			return false;
		}
		GD.Print("Attempt Push");
		if (!_ruleset.isValidPush(_board, previousPosition, selection, currentPlayer)) {
			previousPosition = null;
			return false; 
		}
		bool isSuccess = _board.pushMove(previousPosition, selection);
		previousPosition = null; 
		return isSuccess; 
	}	
	private bool addTile(Location selection, Piece.Color color) {
		if (!_board.isOnBoard(selection))
			return false;
		if (_board.tileAt(selection) != null) return false;
		GD.Print("Attempt Add");
		bool isSuccess = _board.placeTile(new Tile(color), selection);
		previousPosition = null;
		return isSuccess;
		// _board.addTile(new Tile(color), selection);
		// return true;
	}

	#nullable enable
	private bool movePiece(Location selection) {
		if (previousPosition == null) {
			previousPosition = giveValidPreviousSelection(selection);
			return false;
		}
		GD.Print("Attempt Move");
		if (!isValidPieceMove(selection)) { 
			previousPosition = null;
			return false; 
		}
		bool isSuccess = _board.movePiece(previousPosition, selection);
		previousPosition = null; 
		return isSuccess;
	}

	private bool isValidPieceMove(Location selection) {
		if (!_board.isOnBoard(selection)) return false; 
		
		if (_board.tileAt(selection) == null) {
			GD.Print("Checking Tile Move");
			if (!_ruleset.isValidTileMove(_board, previousPosition, selection, currentPlayer)) return false; 
			GD.Print("Good Tile Move");
		}
		else {
			GD.Print("Checking Scout Move");
			if (!_ruleset.isValidScoutMove(_board, previousPosition, selection, currentPlayer)) return false; 
			GD.Print("Good Scout Move");
		}
		
		return true;
	}

	private Location? giveValidPreviousSelection(Location selection) {
		int[] size = _board.size(); 
		if (selection.row() < 0 || selection.row() >= size[0] || selection.column() < 0 || selection.column() >= size[1])
			return null; 
		if (_board.tileAt(selection) == null) 
			return null;
		return selection;
	}
	#nullable disable
}
