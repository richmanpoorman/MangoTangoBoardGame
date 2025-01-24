using Godot;
using System;

// Manages the state of the board, and holds the actual board itself
public partial class BoardManager : Node
{ 
	private Board _board = new GridBoard(7, 5); 
	
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
		Board.Position? selection = selector.selection(); 
		if (selection == null) {
			previousPosition = null;
			return; 
		}

		Board.Position square = selection.Value;
		GD.Print("Selected: ", square.row, ", ", square.column); 


		
		bool finishTurn = false;
		switch(selector.mouseButton()) {
			case MouseButton.Left:
				finishTurn = moveTile(selection.Value);
			break; 
			case MouseButton.Right:
				finishTurn = addTile(selection.Value, currentPlayer);
			break;
		}
		

		onUpdate();

		if (finishTurn) {
			switch(currentPlayer) {
				case Piece.Color.PLAYER_1:
				currentPlayer = Piece.Color.PLAYER_2; 
				break; 
				case Piece.Color.PLAYER_2: 
				currentPlayer = Piece.Color.PLAYER_1;
				break; 
			}
		}
	}
	#nullable disable


	private bool addTile(Board.Position selection, Piece.Color color) {
		if (_board.tileAt(selection) != null) return false;
		_board.addTile(new Tile(color), selection);
		return true;
	}

	#nullable enable
	private bool moveTile(Board.Position selection) {
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
	}
	#nullable disable
}
