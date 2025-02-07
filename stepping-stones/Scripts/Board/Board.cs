
public interface Board
{
    // public struct Location {
	// 	public Location(int _row, int _column) {
	// 		row    = _row; 
	// 		column = _column; 
	// 	}
	// 	public int row { get; }
	// 	public int column { get; }
	// }

    /*size
    Inputs: None
    Returns: Integer array
    Description: returns dimensions of the board, eg [x, y, z]*/
    public int[] size(); 
    /*tileLayer
    Inputs: None
    Returns: 2D Tile array
    Description: returns the tiles on the board in an array representation*/
	public Tile?[,] tileLayer(); 
    /*scoutLayer
    Inputs: None
    Returns: 2D Scout array
    Description: returns the scouts on the board in an array represention*/
    public Scout?[,] scoutLayer();

    #nullable enable
    /*tileAt
    Inputs: Location
    Returns: Tile or null
    Description: returns tile at the given position on the board; if no tile present, returns null*/
    public Tile? tileAt(Location position); 
    /*scoutAt
    Inputs: Location
    Returns: Scout or null
    Description: retursn scout at given position on board; if no scout present, returns null*/
    public Scout? scoutAt(Location position); 
    #nullable disable

    /*addTile
    Inputs: Tile, Location
    Returns: None
    Description: changes tile at given location on board to the given tile*/
    public void addTile(Tile tile, Location position); 
    /*removeTile
    Inputs: Location
    Returns: Tile
    Description: Changes tile at given Location to null; returns tile that was at Location*/
    public Tile removeTile(Location position); 
    /*moveTile
    Inputs: Location from, Location to
    Returns: None
    Description: moves the tile at Location from to Location to*/
    public void moveTile(Location from, Location to); 
    
    /*addScout
    Inputs: Scout, Location
    Returns: None
    Description: Sets given Location on scout layer to given Scout*/
    public void addScout(Scout scout, Location position); 
    /*removeScout
    Inputs: Location
    Returns: Scout
    Description: Sets given position on scout layer to null;
    returns scout that used to be there*/
    public Scout removeScout(Location position); 
    /*moveScout
    Inputs: Location from, Location to
    Returns: None
    Description: Puts the scout at Location from at Location to; the space at from is set to null*/
    public void moveScout(Location from, Location to); 
    public bool isOnBoard(Location location); 
}
