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
    public Tile? tileAt(Board.Position position) { return testTiles[position.row, position.column]; }
    public Scout? scoutAt(Board.Position position) { return testScouts[position.row, position.column]; }
    #nullable disable

    public void addTile(Tile tile, Board.Position position) { GD.Print("Add ", tile.color(), " tile at: (", position.row, ", ", position.column, ")"); }
    public Tile removeTile(Board.Position position) { return null; }
    public void moveTile(Board.Position from, Board.Position to) { GD.Print("Tile From: (", from.row, ", ", from.column, ") to (", to.row, ", ", to.column,")"); }
    
    public void addScout(Scout scout, Board.Position position) { GD.Print("Add ", scout.color(), " tile at: (", position.row, ", ", position.column, ")"); }
    public Scout removeScout(Board.Position position) { return null; }
    public void moveScout(Board.Position from, Board.Position to) { GD.Print("Scout From: (", from.row, ", ", from.column, ") to (", to.row, ", ", to.column,")"); }
}