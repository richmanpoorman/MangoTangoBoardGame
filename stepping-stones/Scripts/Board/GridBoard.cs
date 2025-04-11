using System;
using System.Collections.Generic;
class GridBoard : Board {

    private int[] dimensions = {4, 6};

    #nullable enable
    private Tile?[,] tiles = {}; 
    private Scout?[,] scouts = {}; 
    #nullable disable 
    public GridBoard(int rowCount, int columnCount, IList<Tuple<Tile, Location>> tileList, IList<Tuple<Scout, Location>> scoutList) {
        dimensions[0] = rowCount; 
        dimensions[1] = columnCount; 
        tiles  = new Tile[rowCount, columnCount]; 
        scouts = new Scout[rowCount, columnCount];

        foreach (var (tile, location) in tileList) {
            if (isOnBoard(location)) 
                addTile(tile, location); 
        }
        foreach (var (scout, location) in scoutList) {
            if (isOnBoard(location)) 
                addScout(scout, location); 
        }
    }

    public GridBoard(int rowCount, int columnCount) {
        dimensions[0] = rowCount; 
        dimensions[1] = columnCount; 
        tiles = new Tile[rowCount, columnCount]; 
        scouts = new Scout[rowCount, columnCount];

        // Set up with the two pieces
        tiles[rowCount / 2, 1]          = new Tile(Piece.Color.PLAYER_1);
        tiles[rowCount / 2, columnCount - 2]  = new Tile(Piece.Color.PLAYER_2);
        scouts[rowCount / 2, 1]         = new Scout(Piece.Color.PLAYER_1);
        scouts[rowCount / 2, columnCount - 2] = new Scout(Piece.Color.PLAYER_2);
    }

    public int[] size() { return dimensions; }
	public Tile?[,] tileLayer() { return tiles; }
    public Scout?[,] scoutLayer() { return scouts; }

    #nullable enable
    public Tile? tileAt(Location position) { return tiles[position.row(), position.column()]; }
    #nullable disable

    #nullable enable
    public Scout? scoutAt(Location position) { return scouts[position.row(), position.column()]; }
    #nullable disable

    public void addTile(Tile tile, Location position) { tiles[position.row(), position.column()] = tile; }
    public Tile removeTile(Location position) {
        Tile tile = tiles[position.row(), position.column()]; 
        tiles[position.row(), position.column()] = null; 
        return tile; 
    }
    public void moveTile(Location from, Location to) {
        tiles[to.row(), to.column()] = tiles[from.row(), from.column()]; 
        tiles[from.row(), from.column()] = null; 
    }
    
    public void addScout(Scout scout, Location position) { scouts[position.row(), position.column()] = scout; }
    public Scout removeScout(Location position) {
        Scout scout = scouts[position.row(), position.column()]; 
        scouts[position.row(), position.column()] = null; 
        return scout; 
    }
    public void moveScout(Location from, Location to) {
        scouts[to.row(), to.column()] = scouts[from.row(), from.column()]; 
        scouts[from.row(), from.column()] = null; 
    }

    public bool isOnBoard(Location location)
    {
        return 0 <= location.row() && location.row() < size()[0] && 0 <= location.column() && location.column() < size()[1]; 
    }

}