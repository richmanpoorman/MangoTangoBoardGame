using Godot;
using System.Data;

class TestBoard : Board {


    private Tile[,] testTiles = {
        {new Tile(Piece.Color.PLAYER_1), null, new Tile(Piece.Color.PLAYER_1)},
        {new Tile(Piece.Color.PLAYER_2), null, new Tile(Piece.Color.PLAYER_2)}
    }; 

    private Scout[,] testScouts = {
        {null, null, new Scout(Piece.Color.PLAYER_1)}, 
        {null, null, new Scout(Piece.Color.PLAYER_2)}
    };

    public TestBoard() {
    }

    public int[] size() { 
        int[] dimensions = {testTiles.GetLength(0), testTiles.GetLength(1)}; 
        return dimensions; 
    }
	public Tile[,] tileLayer() { return testTiles; }
    public Scout[,] scoutLayer() { return testScouts; }
    
    #nullable enable
    public Tile? tileAt(Location position) { return testTiles[position.row(), position.column()]; }
    public Scout? scoutAt(Location position) { return testScouts[position.row(), position.column()]; }
    #nullable disable

    public void addTile(Tile tile, Location position) { GD.Print("Add ", tile.color(), " tile at: (", position.row(), ", ", position.column(), ")"); }
    public Tile removeTile(Location position) { return null; }
    public void moveTile(Location from, Location to) { GD.Print("Tile From: (", from.row(), ", ", from.column(), ") to (", to.row(), ", ", to.column(),")"); }
    
    public void addScout(Scout scout, Location position) { GD.Print("Add ", scout.color(), " tile at: (", position.row(), ", ", position.column(), ")"); }
    public Scout removeScout(Location position) { return null; }
    public void moveScout(Location from, Location to) { GD.Print("Scout From: (", from.row(), ", ", from.column(), ") to (", to.row(), ", ", to.column(),")"); }

    public bool isOnBoard(Location location)
    {
        return 0 <= location.row() && location.row() < size()[0] && 0 <= location.column() && location.column() < size()[1]; 
    }

}