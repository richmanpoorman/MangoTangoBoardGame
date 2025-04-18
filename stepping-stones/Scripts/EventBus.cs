using Godot;
using Godot.Collections; 

public partial class EventBus : Node
{
    public static EventBus Bus { get; private set; }
	public static EventBus gdscriptBus() {
		return Bus;
	}

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
	// If joining a room online
	[Signal] 
	public delegate void onJoinRoomEventHandler(string roomCode); 

	// If making a room for online
	[Signal]
	public delegate void onMakeRoomEventHandler(); 

	// If a player wins
    [Signal]
	public delegate void onPlayerWinEventHandler(); 

    // When the board is wiped clean
	[Signal]
	public delegate void onBoardResetEventHandler(); 

    // When the turn changes
	[Signal]
	public delegate void onTurnChangeEventHandler(PlayerColor turn); 

    // When the phase of the game changes
	[Signal]
	public delegate void onPhaseStartEventHandler(GamePhase phase); 

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
	public delegate void onSelectionEventHandler(PlayerColor player, int row, int column);

	// When a new tileset is created
	[Signal]
	public delegate void onChangePieceTilesetEventHandler(TileSet newSprites, Dictionary<PlayerColor, Dictionary<PieceType, int>> tilesetIDs);

	// When a new player joins the game (be it from online, user, or AI), as well as the color
	[Signal]
	public delegate void onPlayerJoinEventHandler(PlayerColor player, PlayerType selectorType);

	[Signal]
	public delegate void onPlayerLeaveEventHandler(PlayerColor player);


	// Connects associated signals, which should also trigger if one is sent
	private void connectSignals() {
		onTileMove   += () => EmitSignal(SignalName.onBoardUpdate);
		onTilePlace  += () => EmitSignal(SignalName.onBoardUpdate);
		onTilePush   += () => EmitSignal(SignalName.onBoardUpdate);
		onBoardReset += () => EmitSignal(SignalName.onBoardUpdate);
	}
}
