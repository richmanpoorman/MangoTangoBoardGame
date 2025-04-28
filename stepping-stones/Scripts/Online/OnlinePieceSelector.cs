
using Godot;

public partial class OnlinePieceSelector : Node2D, MoveSelector {
    private PlayerColor _player;
    private Location _selection;

    private EventBus _eventBus; 

	// public void setBoardManager(BoardManager manager) { boardManager = manager; }

    public override void _Ready() {
		_eventBus = EventBus.Bus;
		_eventBus.onSelection += receiveInputLocalMachine; 
	}
    public void setPlayer(PlayerColor playerColor) { _player = playerColor; }
    public PlayerColor player() {return _player;} // Which player this selector selects moves for
    public Location selection() {return _selection;} 
    public void emitMove() {
        GD.Print("Don't call online emit move");
        return; 
    }

    public override void _ExitTree() {
        _eventBus.onSelection -= receiveInputLocalMachine; 
    }

    private void receiveInputLocalMachine(PlayerColor player, int row, int column) {
        if (player == _player) return; 
        GD.Print($" >> {_player} sent {player} moves {row}, {column}");
        Rpc(MethodName.sendOutputOnOtherMachine, (int)player, row, column);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void sendOutputOnOtherMachine (PlayerColor player, int row, int column) {
        GD.Print($" << {_player} received {_player} moves {row}, {column}");
        EventBus.Bus.EmitSignal(EventBus.SignalName.onSelection, (int)player, row, column);
    }
}