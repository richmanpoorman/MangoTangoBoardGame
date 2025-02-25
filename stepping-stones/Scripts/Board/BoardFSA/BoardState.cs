using Godot; 

public interface BoardState {
    public BoardState processInput(SteppingStonesBoard board, Rules ruleset, Location location); 
    public Piece.Color playerTurn();
}