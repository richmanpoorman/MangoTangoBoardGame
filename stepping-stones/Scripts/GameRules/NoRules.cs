using System.Collections;
using System.Collections.Generic;

public class NoRules : Rules
{

    public NoRules() {}
    public bool hasWon(Board board, Piece.Color playerTurn)
    {
        return false; 
    }

    public bool isValidPlace(Board board, Location place, Piece.Color playerTurn)
    {
        return true;
    }

    public bool isValidPush(Board board, Location from, Location to, Piece.Color playerTurn)
    {
        return true;
    }

    public bool isValidScoutMove(Board board, Location from, Location to, Piece.Color playerTurn)
    {
        return true;
    }

    public bool isValidTileMove(Board board, Location from, Location to, Piece.Color playerTurn)
    {
        return true;
    }

    public IList<Rules.ValidMove> legalOptions(Board board, Location start, Piece.Color playerTurn)
    {
        return new List<Rules.ValidMove>(); 
    }
}