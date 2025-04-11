using Godot; 

public interface BoardState {
    public BoardState processInput(SteppingStonesBoard board, Rules ruleset, Location location); 
    public PlayerColor playerTurn();

    public void changeTurn(PlayerColor turn);
}