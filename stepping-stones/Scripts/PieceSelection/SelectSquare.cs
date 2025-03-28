using Godot;
using System;

public partial class SelectSquare : Node2D, MoveSelector
{
	[Export]
	private Piece.Color _player = Piece.Color.PLAYER_1; 
	
	private Piece.Color _currentTurn = Piece.Color.PLAYER_1;

	public enum ClickType {
		LEFT, RIGHT, MIDDLE, NONE
	}

	// [Export]
	// private BoardManager boardManager; 

	[Export]
	private TileMapLayer selectionGrid; 


	private Location _position;
	private MouseButton _mouseButton = MouseButton.None;
	private EventBus _eventBus; 

	// public void setBoardManager(BoardManager manager) { boardManager = manager; }

    public override void _Ready() {
		_eventBus = EventBus.Bus;
		_eventBus.onTurnChange += onTurnChange; 
	}

	public void onTurnChange(Piece.Color turn) { _currentTurn = turn; }

    public override void _Input(InputEvent @event)
    {
		if (_currentTurn != _player) return; // Only try to process on your turn (only needed because local shares a board)
		// GD.Print("Event");
		if (@event is not InputEventMouseButton) return; 

		InputEventMouseButton mouseEvent = (InputEventMouseButton)@event; 
		
		if (!mouseEvent.Pressed) return; 
		// GD.Print("Mouse Down");
		
		Vector2I square = selectionGrid.LocalToMap(GetLocalMousePosition());
		_position = Location.at(square.Y, square.X);

		_mouseButton = mouseEvent.ButtonIndex;

		if (selection() is Location position)
			GD.Print("PLAYER: ", _player, " has Mouse Result: ", position.row(), ", ", position.column());
		else 
			GD.Print("PLAYER: ", _player, "Mouse Result: null");
		
		emitMove(); 
    }

	public Location selection() { return _position; }
	public MouseButton mouseButton() { return _mouseButton; }

    public Piece.Color player() { return _player; }

    public void emitMove() { _eventBus.EmitSignal(EventBus.SignalName.onSelection, (int)_player, _position.row(), _position.column()); }

    public void setPlayer(Piece.Color playerColor) { _player = playerColor; }
}
