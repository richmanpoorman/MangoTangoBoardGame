using System.Reflection;

public interface BoardStateMachine {

    public BoardState currentState(); 

    public void selectSquare(Location location);

    public void changeState(BoardState state);

    public void changeBoard(SteppingStonesBoard board); 

    public void changeTurn(PlayerColor turn);
    
    public void changeRules(Rules rules);

    public SteppingStonesBoard board();
    public Rules rules();  
    
    public PlayerColor turn();

}