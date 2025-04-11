using Godot;
using System;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

public partial class EventBus : Node
{
    public static EventBus Bus { get; private set; }

    public override void _EnterTree()
    {
        if (Bus != null)
        {
            GD.PushWarning("Attempted to re-create another instance of signal bus!");
            return;
        }

        Bus = this;
		GD.Print("Bus Initialized");
		
		connectSignals(); 
		 
    }
	

    /*
     *    SIGNALS
     */
	[Signal] 
	public delegate void onJoinRoomEventHandler(string roomCode); 

	[Signal]
	public delegate void onMakeRoomEventHandler(); 

    [Signal]
	public delegate void onPlayerWinEventHandler(); 

    // When the board is wiped clean
	[Signal]
	public delegate void onBoardResetEventHandler(); 

    // When the turn changes
	[Signal]
	public delegate void onTurnChangeEventHandler(Piece.Color turn); 

    // When the phase of the game changes
	[Signal]
	public delegate void onPhaseStartEventHandler(BoardManager.GamePhase phase); 

    // Whenever it is possible for the board to update in any way
	[Signal]
	public delegate void onBoardUpdateEventHandler();

    // Whenever the a tile is placed
	[Signal]
	public delegate void onTilePlaceEventHandler(); 

    // Whenever a scout has moved
	[Signal]
	public delegate void onScoutMoveEventHandler(); 

    // Whenever a tile is moved
	[Signal]
	public delegate void onTileMoveEventHandler(); 

    // Whenever the tiles are pushed
	[Signal]
	public delegate void onTilePushEventHandler(); 

	[Signal]
	public delegate void onGameResetEventHandler();

    // When a square is selected
    [Signal]
	public delegate void onSelectionEventHandler(Piece.Color player, int row, int column);

	// Connects associated signals, which should also trigger if one is sent
	private void connectSignals() {
		onTileMove   += () => EmitSignal(SignalName.onBoardUpdate);
		onTilePlace  += () => EmitSignal(SignalName.onBoardUpdate);
		onTilePush   += () => EmitSignal(SignalName.onBoardUpdate);
		onBoardReset += () => EmitSignal(SignalName.onBoardUpdate);
	}
}
