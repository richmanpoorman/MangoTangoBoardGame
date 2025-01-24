

public interface SteppingStonesBoard : Board {

    public enum PushDirection {
        UP, DOWN, LEFT, RIGHT
    }

    public Board board(); 

    public bool placeTile(Tile tile, Board.Position position);
    public bool movePiece(Board.Position from, Board.Position to); 

    public bool pushMove(Board.Position start, Board.Position to); 

    public bool pushDirection(Board.Position start, PushDirection direction); 

}