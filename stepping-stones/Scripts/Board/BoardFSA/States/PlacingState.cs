

public class PlacingState : BoardState
{
    private int p1Count, p2Count; 
    private Piece.Color turn; 
    public PlacingState(int player1Count, int player2Count, Piece.Color playerTurn) {
        p1Count = player1Count;
        p2Count = player2Count; 
        turn = playerTurn; 
    }

    public Piece.Color playerTurn() { return turn; }

    public BoardState processInput(SteppingStonesBoard board, Rules ruleset, Location location)
    {
        bool isSuccessfulPiecePlacement = placePiece(board, ruleset, location); 
        if (!isSuccessfulPiecePlacement) return this; // If it wasn't successful, don't move on
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
        return new MovementState(); 
         
    }

    private bool placePiece(SteppingStonesBoard board, Rules ruleset, Location location) {
        if (!board.isOnBoard(location)) return false; 
        if (!ruleset.isValidPlace(board, location, turn)) return false; 
        bool isSuccess = board.placeTile(new Tile(turn), location);
        if (!isSuccess) return false; 
        turn = (turn == Piece.Color.PLAYER_1) ? Piece.Color.PLAYER_2 : Piece.Color.PLAYER_1;
        return true;
    }
}