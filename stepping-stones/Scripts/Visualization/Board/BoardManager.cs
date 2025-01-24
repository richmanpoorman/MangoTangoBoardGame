using Godot;
using System;
using System.Reflection.Metadata.Ecma335;

// Manages the state of the board, and holds the actual board itself
public partial class BoardManager : Node
{ 
	private SteppingStonesBoard _board = new GridSteppingStonesBoard(7, 5); // new GridBoard(7, 5); 
	
	private Piece.Color currentPlayer = Piece.Color.PLAYER_1; 
	private Board.Position? previousPosition = null; 

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
		Board.Position selection = selector.selection(); 

		GD.Print("Selection: (", selection.row, ", ", selection.column, ")");

		if (previousPosition is Board.Position prev) 
			GD.Print("Previous: (", prev.row, ", ", prev.column, ")");
		else 	
			GD.Print("Previous: null");	
		
		bool finishTurn = false;
		switch(selector.mouseButton()) {
			case MouseButton.Left:
				finishTurn =  moveTile(selection);
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

	private bool pushPieces(Board.Position selection) {
		if (previousPosition == null) {
			previousPosition = giveValidPreviousSelection(selection);
			return false;
		}
		GD.Print("Attempt Push");
		bool isSuccess = _board.pushMove(previousPosition.Value, selection);
		previousPosition = null; 
		return isSuccess; 
	}	
	private bool addTile(Board.Position selection, Piece.Color color) {
		int[] size = _board.size();
		if (selection.row < 0 || selection.row >= size[0] || selection.column < 0 || selection.column >= size[1])
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
	private bool moveTile(Board.Position selection) {
		if (previousPosition == null) {
			previousPosition = giveValidPreviousSelection(selection);
			return false;
		}
		GD.Print("Attempt Move");
		bool isSuccess = _board.movePiece(previousPosition.Value, selection);
		previousPosition = null; 
		return isSuccess;
		/*
		if (previousPosition == null) {
			previousPosition = selection;
			return false;
		}
		Board.Position prev = previousPosition.Value;
		
		// If the selection is the same exact square, ignore
		if (prev.row == selection.row && prev.column == selection.column) {
			previousPosition = null;
			return false;
		}

		GD.Print("Previous: ", prev.row, ", ", prev.column);

		Tile? tile = _board.tileAt(prev); 
		if (tile == null) {
			previousPosition = null;
			return false; 
		}
		
		GD.Print("Tile was not null");

		if (_board.tileAt(selection) == null) {
			_board.moveTile(prev, selection);
			GD.Print("Moved Tile");
		}
		if (_board.scoutAt(previousPosition.Value) is Scout) {
			_board.moveScout(previousPosition.Value, selection);
			GD.Print("Moved Scout");
		}
		
		previousPosition = null;
		return true;
		*/
	}

	private Board.Position? giveValidPreviousSelection(Board.Position selection) {
		int[] size = _board.size(); 
		if (selection.row < 0 || selection.row >= size[0] || selection.column < 0 || selection.column >= size[1])
			return null; 
		if (_board.tileAt(selection) == null) 
			return null;
		return selection;
	}
	#nullable disable
}
