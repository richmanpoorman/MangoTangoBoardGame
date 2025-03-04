public class MovementState : BoardState
{
    private Piece.Color turn;
    public MovementState(Piece.Color playerTurn) {
        turn = playerTurn; 
    }

    public void changeTurn(Piece.Color _turn) { turn = _turn; }


    public Piece.Color playerTurn() { return turn; }

    public BoardState processInput(SteppingStonesBoard board, Rules ruleset, Location location)
    {
        throw new System.NotImplementedException();
    }
}