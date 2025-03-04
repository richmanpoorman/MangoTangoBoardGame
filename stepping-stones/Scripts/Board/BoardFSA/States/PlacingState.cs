

using System.Collections.Generic;

public class PlacingState : BoardState
{
    private Dictionary<Piece.Color, int> pieceCounts; 
    private Piece.Color turn; 
    private EventBus _eventBus; 
    public PlacingState(Dictionary<Piece.Color, int> tileCounts, Piece.Color playerTurn) {
        pieceCounts = tileCounts;
        turn = playerTurn; 
        _eventBus = EventBus.Bus; 
        _eventBus.EmitSignal(EventBus.SignalName.onPhaseStart, (int)BoardManager.GamePhase.PLACE);
    }

    public Piece.Color playerTurn() { return turn; }

    public BoardState processInput(SteppingStonesBoard board, Rules ruleset, Location location)
    {
        bool isSuccessfulPiecePlacement = placePiece(board, ruleset, location); 
        if (!isSuccessfulPiecePlacement) return this; // If it wasn't successful, don't move on

        turn = otherTurnPlayer();
        _eventBus.EmitSignal(EventBus.SignalName.onTilePlace);
        _eventBus.EmitSignal(EventBus.SignalName.onTurnChange, (int)turn);

        int p1Count = pieceCounts[Piece.Color.PLAYER_1], p2Count = pieceCounts[Piece.Color.PLAYER_2];

        if (p1Count != 0 && p2Count != 0) return this; // If either can place, let them place

         // If only one player has tiles left, keep going to them
        if (p1Count == 0 && p2Count != 0) {
            turn = Piece.Color.PLAYER_2;
            return this;
        }
        if (p1Count != 0 && p2Count == 0) {
            turn = Piece.Color.PLAYER_1;
            return this;
        }

        // Otherwise go to the movement state
        _eventBus.EmitSignal(EventBus.SignalName.onPhaseStart, (int)BoardManager.GamePhase.MOVE);
        return new MovementState(turn); 
         
    }
    public void setTileCount(Piece.Color player, int count) { pieceCounts[player] = count; }
    public int tileCount(Piece.Color player) { return pieceCounts[player]; }

    private bool placePiece(SteppingStonesBoard board, Rules ruleset, Location location) {
        if (!board.isOnBoard(location)) return false; 
        if (!ruleset.isValidPlace(board, location, turn)) return false; 
        bool isSuccess = board.placeTile(new Tile(turn), location);
        if (!isSuccess) return false; 
        turn = (turn == Piece.Color.PLAYER_1) ? Piece.Color.PLAYER_2 : Piece.Color.PLAYER_1;
        return true;
    }

    private Piece.Color otherTurnPlayer() {
        switch(turn) {
            case Piece.Color.PLAYER_1:
                return Piece.Color.PLAYER_2; 
            case Piece.Color.PLAYER_2: 
                return Piece.Color.PLAYER_1; 
        }
        return Piece.Color.PLAYER_1;
    }

    public void changeTurn(Piece.Color _turn) { turn = _turn; }

}