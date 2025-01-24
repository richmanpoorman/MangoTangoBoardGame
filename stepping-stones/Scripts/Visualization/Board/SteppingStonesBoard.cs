

public interface SteppingStonesBoard : Board {

    public enum PushDirection {
        UP, DOWN, LEFT, RIGHT
    }

    public Board board(); 

    public bool placeTile(Tile tile, Location position);
    public bool movePiece(Location from, Location to); 

    public bool pushMove(Location start, Location to); 

    public bool pushDirection(Location start, PushDirection direction); 

}