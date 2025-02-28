using Godot;
using System;

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
    }

    /*
     *    SIGNALS
     */

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

    // When a square is selected
    [Signal]
	public delegate void onSelectionEventHandler(int row, int column);

    
}
