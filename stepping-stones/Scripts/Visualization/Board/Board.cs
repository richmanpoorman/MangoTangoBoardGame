
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

    public int[] size(); 
	public Tile[,] tileLayer(); 
    public Scout[,] scoutLayer();

    #nullable enable
    public Tile? tileAt(Location position); 
    public Scout? scoutAt(Location position); 
    #nullable disable

    public void addTile(Tile tile, Location position); 
    public Tile removeTile(Location position); 
    public void moveTile(Location from, Location to); 
    
    public void addScout(Scout scout, Location position); 
    public Scout removeScout(Location position); 
    public void moveScout(Location from, Location to); 
    
}
