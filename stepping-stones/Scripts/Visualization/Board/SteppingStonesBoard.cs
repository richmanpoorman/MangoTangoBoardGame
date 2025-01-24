

public interface SteppingStonesBoard {

    public enum PushDirection {
        UP, DOWN, LEFT, RIGHT
    }

    public Board board(); 

    public bool addTile(Tile tile, Board.Position position);
    public bool movePiece(Board.Position from, Board.Position to); 

    public bool pushMove(Board.Position start, Board.Position to); 

    public bool pushOff(Board.Position start, PushDirection direction); 

}