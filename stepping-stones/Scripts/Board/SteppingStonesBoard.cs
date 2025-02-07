

public interface SteppingStonesBoard : Board {

    public enum PushDirection {
        UP, DOWN, LEFT, RIGHT
    }
	
    /*board
	Inputs: None
	Returns: Board
	Description: returns internal board*/
    public Board board(); 

	/*placeeTile
	Inputs: Tile, Location
	Returns: bool
	Description: Attempts to add Tile at Location; returns true if sucessful, false if not*/
    public bool placeTile(Tile tile, Location position);
	/*movePiece
	Inputs: Location from, Location to
	Returns: bool
	Description: Attempts to move piece from Location from to Location to; returns sucess of attempt*/
    public bool movePiece(Location from, Location to); 

	/*pushMove
	Inputs: Location start, Location to
	Returns: bool
	Description: Attempts to push pieces in direction from start towards to; returns sucess of attempt*/
    public bool pushMove(Location start, Location to); 

	/*pushDirection
	Inputs: Location, PushDirection
	Returns: bool
	Description: Attempts to push tile from start in direction of PushDirection; returns sucess of attempt*/
    public bool pushDirection(Location start, PushDirection direction); 

}