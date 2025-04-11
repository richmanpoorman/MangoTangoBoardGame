public class MovementState : BoardState
{
    private PlayerColor turn;
    public MovementState(PlayerColor playerTurn) {
        turn = playerTurn; 
    }

    public void changeTurn(PlayerColor _turn) { turn = _turn; }


    public PlayerColor playerTurn() { return turn; }

    public BoardState processInput(SteppingStonesBoard board, Rules ruleset, Location location)
    {
        throw new System.NotImplementedException();
    }
}