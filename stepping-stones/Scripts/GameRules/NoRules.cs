using System.Collections;
using System.Collections.Generic;

public class NoRules : Rules
{

    public NoRules() {}
    public bool hasWon(Board board, PlayerColor playerTurn)
    {
        return false; 
    }

    public bool isValidPlace(Board board, Location place, PlayerColor playerTurn)
    {
        return true;
    }

    public bool isValidPush(Board board, Location from, Location to, PlayerColor playerTurn)
    {
        return true;
    }

    public bool isValidScoutMove(Board board, Location from, Location to, PlayerColor playerTurn)
    {
        return true;
    }

    public bool isValidTileMove(Board board, Location from, Location to, PlayerColor playerTurn)
    {
        return true;
    }

    public IList<Rules.ValidMove> legalOptions(Board board, Location start, PlayerColor playerTurn)
    {
        return new List<Rules.ValidMove>(); 
    }
}