
public interface Board
{
    public struct Position {
		public Position(int _row, int _column) {
			row    = _row; 
			column = _column; 
		}
		public int row { get; }
		public int column { get; }
	}

    public int[] size(); 
	public Tile[,] tileLayer(); 
    public Scout[,] scoutLayer();

    #nullable enable
    public Tile? tileAt(Position position); 
    public Scout? scoutAt(Position position); 
    #nullable disable

    public void addTile(Tile tile, Position position); 
    public Tile removeTile(Position position); 
    public void moveTile(Position from, Position to); 
    
    public void addScout(Scout scout, Position position); 
    public Scout removeScout(Position position); 
    public void moveScout(Position from, Position to); 
    
}
