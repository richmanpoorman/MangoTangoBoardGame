

using System.Collections.Generic;

public class PlacingState : BoardState
{
    private Dictionary<PlayerColor, int> pieceCounts; 
    private PlayerColor turn; 
    private EventBus _eventBus; 
    public PlacingState(Dictionary<PlayerColor, int> tileCounts, PlayerColor playerTurn) {
        pieceCounts = tileCounts;
        turn = playerTurn; 
        _eventBus = EventBus.Bus; 
        _eventBus.EmitSignal(EventBus.SignalName.onPhaseStart, (int)GamePhase.PLACE);
    }

    public PlayerColor playerTurn() { return turn; }

    public BoardState processInput(SteppingStonesBoard board, Rules ruleset, Location location)
    {
        bool isSuccessfulPiecePlacement = placePiece(board, ruleset, location); 
        if (!isSuccessfulPiecePlacement) return this; // If it wasn't successful, don't move on

        turn = otherTurnPlayer();
        _eventBus.EmitSignal(EventBus.SignalName.onTilePlace);
        _eventBus.EmitSignal(EventBus.SignalName.onTurnChange, (int)turn);

        int p1Count = pieceCounts[PlayerColor.PLAYER_1], p2Count = pieceCounts[PlayerColor.PLAYER_2];

        if (p1Count != 0 && p2Count != 0) return this; // If either can place, let them place

         // If only one player has tiles left, keep going to them
        if (p1Count == 0 && p2Count != 0) {
            turn = PlayerColor.PLAYER_2;
            return this;
        }
        if (p1Count != 0 && p2Count == 0) {
            turn = PlayerColor.PLAYER_1;
            return this;
        }

        // Otherwise go to the movement state
        _eventBus.EmitSignal(EventBus.SignalName.onPhaseStart, (int)GamePhase.MOVE);
        return new MovementState(turn); 
         
    }
    public void setTileCount(PlayerColor player, int count) { pieceCounts[player] = count; }
    public int tileCount(PlayerColor player) { return pieceCounts[player]; }

    private bool placePiece(SteppingStonesBoard board, Rules ruleset, Location location) {
        if (!board.isOnBoard(location)) return false; 
        if (!ruleset.isValidPlace(board, location, turn)) return false; 
        bool isSuccess = board.placeTile(new Tile(turn), location);
        if (!isSuccess) return false; 
        turn = (turn == PlayerColor.PLAYER_1) ? PlayerColor.PLAYER_2 : PlayerColor.PLAYER_1;
        return true;
    }

    private PlayerColor otherTurnPlayer() {
        switch(turn) {
            case PlayerColor.PLAYER_1:
                return PlayerColor.PLAYER_2; 
            case PlayerColor.PLAYER_2: 
                return PlayerColor.PLAYER_1; 
        }
        return PlayerColor.PLAYER_1;
    }

    public void changeTurn(PlayerColor _turn) { turn = _turn; }

}