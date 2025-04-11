
public class EmptyBoard : SteppingStonesBoard
{
    
    private int[] dimensions = {4, 6};

    #nullable enable
    private Tile?[,] tiles = {}; 
    private Scout?[,] scouts = {}; 

    public void addScout(Scout scout, Location position)
    {
        throw new System.NotImplementedException();
    }

    public void addTile(Tile tile, Location position)
    {
        throw new System.NotImplementedException();
    }

    public Board board()
    {
        throw new System.NotImplementedException();
    }

    public bool isOnBoard(Location location)
    {
        throw new System.NotImplementedException();
    }

    public bool movePiece(Location from, Location to)
    {
        throw new System.NotImplementedException();
    }

    public void moveScout(Location from, Location to)
    {
        throw new System.NotImplementedException();
    }

    public void moveTile(Location from, Location to)
    {
        throw new System.NotImplementedException();
    }

    public bool placeTile(Tile tile, Location position)
    {
        throw new System.NotImplementedException();
    }

    public bool pushDirection(Location start, SteppingStonesBoard.PushDirection direction)
    {
        throw new System.NotImplementedException();
    }

    public bool pushMove(Location start, Location to)
    {
        throw new System.NotImplementedException();
    }

    public Scout removeScout(Location position)
    {
        throw new System.NotImplementedException();
    }

    public Tile removeTile(Location position)
    {
        throw new System.NotImplementedException();
    }

    public Scout scoutAt(Location position)
    {
        throw new System.NotImplementedException();
    }

    public Scout[,] scoutLayer()
    {
        throw new System.NotImplementedException();
    }

    public int[] size()
    {
        throw new System.NotImplementedException();
    }

    public Tile tileAt(Location position)
    {
        throw new System.NotImplementedException();
    }

    public Tile[,] tileLayer()
    {
        throw new System.NotImplementedException();
    }
}