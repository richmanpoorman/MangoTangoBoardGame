public interface BoardStateMachine {

    public BoardState currentState(); 

    public void selectSquare(Location location);

    public void changeState(BoardState state);

    public void changeBoard(SteppingStonesBoard board); 
    
    public void changeRules(Rules rules);

    public SteppingStonesBoard board();
    public Rules rules();  

}