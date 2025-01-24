using Godot;
using System;

public partial class SelectSquare : Node2D
{

	public enum ClickType {
		LEFT, RIGHT, MIDDLE, NONE
	}

	[Export]
	private BoardManager boardManager; 

	[Export]
	private TileMapLayer selectionGrid; 

	private Board board;

	private Board.Position _position;
	private MouseButton _mouseButton = MouseButton.None;

    public override void _Ready() { board = boardManager.board(); }

    public override void _Input(InputEvent @event)
    {
		// GD.Print("Event");
		if (@event is not InputEventMouseButton) return; 

		InputEventMouseButton mouseEvent = (InputEventMouseButton)@event; 
		
		if (!mouseEvent.Pressed) return; 
		// GD.Print("Mouse Down");
		
		Vector2I square = selectionGrid.LocalToMap(GetLocalMousePosition());
		_position = new Board.Position(square.Y, square.X);

		_mouseButton = mouseEvent.ButtonIndex;

		if (selection() is Board.Position position)
			GD.Print("Mouse Result: ", position.row, ", ", position.column);
		else 
			GD.Print("Mouse Result: null");
		boardManager.onSelection();
    }

	public Board.Position selection() { return _position; }
	public MouseButton mouseButton() { return _mouseButton; }
}
