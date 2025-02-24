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

	[Signal]
	public delegate void onSelectionEventHandler(int row, int column);

	private Board board;

	private Location _position;
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
		_position = Location.at(square.Y, square.X);

		_mouseButton = mouseEvent.ButtonIndex;

		if (selection() is Location position)
			GD.Print("Mouse Result: ", position.row(), ", ", position.column());
		else 
			GD.Print("Mouse Result: null");
		EmitSignal(SignalName.onSelection, _position.row(), _position.column()); 
    }

	public Location selection() { return _position; }
	public MouseButton mouseButton() { return _mouseButton; }
}
