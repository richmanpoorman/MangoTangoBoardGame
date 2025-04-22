
using Godot;

public partial class OnlinePieceSelector : Node2D, MoveSelector {
    private PlayerColor _player;
    private Location _selection;

    private EventBus _eventBus; 

	// public void setBoardManager(BoardManager manager) { boardManager = manager; }

    public override void _Ready() {
		_eventBus = EventBus.Bus;
		_eventBus.onSelection += selectionWrapper; 
	}
    public void setPlayer(PlayerColor playerColor) { _player = playerColor; }
    public PlayerColor player() {return _player;} // Which player this selector selects moves for
    public Location selection() {return _selection;} 
    public void emitMove() {
        GD.Print("Don't call online emit move");
        return; 
    }

    public override void _ExitTree() {
        _eventBus.onSelection -= synchronizeSelection; 
    }

    private void selectionWrapper(PlayerColor player, int row, int column) {
        Rpc(MethodName.synchronizeSelection, (int)player, row, column);
    }

    [Rpc]
    public void synchronizeSelection (PlayerColor player, int row, int column) {
        EventBus.Bus.EmitSignal(EventBus.SignalName.onSelection, (int)player, row, column);
    }
}