using System;
using System.Collections.Generic;
using Godot;

public class WeightedScoutRules : Rules 
{
    Rules baseRules = new BasicRules();
    private int SCOUT_WEIGHT = 1; 
    public WeightedScoutRules() {}

    public bool hasWon(Board board, Piece.Color playerTurn)
    {
        return baseRules.hasWon(board, playerTurn);
    }

    public bool isValidPlace(Board board, Location place, Piece.Color playerTurn)
    {
        return baseRules.isValidPlace(board, place, playerTurn);
    }

    public bool isValidPush(Board board, Location from, Location to, Piece.Color playerTurn) {
        if (!board.isOnBoard(from)) return false; // If out of bounds start, it is not valid
        if (from.row() == to.row() && from.column() == to.column()) return false; 
        if (from.row() != to.row() && from.column() != to.column()) return false; 
        if (board.isOnBoard(to) && board.tileAt(to) != null) return false; 

        bool addingCurrentPlayer = true; 
        int tileSum = 0; 
        for (Vector2I position  = new Vector2I(from.row(), from.column()), 
                      end       = new Vector2I(to.row(), to.column()), 
                      direction = new Vector2I(Math.Sign(to.row() - from.row()), Math.Sign(to.column() - from.column())); 
             position != end; 
             position += direction) {

            Location pos = Location.at(position.X, position.Y); 
            if (!board.isOnBoard(pos)) break; 
            Tile? tile = board.tileAt(pos);
            
            if (tile == null) return false; // No breaks allowed 
            if ((playerTurn == tile.color()) != addingCurrentPlayer) { // If tile seen is not the correct color being accumulated
                if (addingCurrentPlayer) addingCurrentPlayer = false; // see opponent tile when counting ours; switch to counting theirs
                else return false; // See our tile when counting opponent tile; NOT ALLOWED
            }

            bool currentTileHasScout = board.scoutAt(pos) != null;


            if (addingCurrentPlayer) tileSum += currentTileHasScout ?  1 + SCOUT_WEIGHT : 1; 
            else tileSum -= currentTileHasScout ? 1 + SCOUT_WEIGHT : 1; 
        }

        if (addingCurrentPlayer) return false; // Need to push at least one opponent tile 
        if (tileSum != 1) return false; // Need to have one more player tile then opponent tile
        
        return true;
    }

    public bool isValidScoutMove(Board board, Location from, Location to, Piece.Color playerTurn)
    {
        return baseRules.isValidScoutMove(board, from, to, playerTurn); 
    }

    public bool isValidTileMove(Board board, Location from, Location to, Piece.Color playerTurn)
    {
        return baseRules.isValidPieceMove(board, from, to, playerTurn);
    }

    public IList<Rules.ValidMove> legalOptions(Board board, Location start, Piece.Color playerTurn)
    {
        List<Rules.ValidMove> moves = new List<Rules.ValidMove>(); 

        // The adjacent cells piece movement
        Location[] moveToList = {start.left(), start.right(), start.up(), start.down()}; 
        foreach (Location moveTo in moveToList) {
            if (board.isOnBoard(moveTo)) {
                if (board.tileAt(moveTo) == null) {
                    if (isValidTileMove(board, start, moveTo, playerTurn))
                        moves.Add(new Rules.ValidMove(moveTo, Rules.MoveType.TILE_MOVE)); 
                }
                else {
                    if (isValidScoutMove(board, start, moveTo, playerTurn))
                        moves.Add(new Rules.ValidMove(moveTo, Rules.MoveType.SCOUT_MOVE)); 
                }
            }
        }

        // left push
        Location current = start; 
        while (board.isOnBoard(current) && board.tileAt(current) != null) current = current.left(); 
        if (isValidPush(board, start, current, playerTurn)) moves.Add(new Rules.ValidMove(current, Rules.MoveType.TILE_PUSH_LEFT)); 

        // right push
        current = start; 
        while (board.isOnBoard(current) && board.tileAt(current) != null) current = current.right(); 
        if (isValidPush(board, start, current, playerTurn)) moves.Add(new Rules.ValidMove(current, Rules.MoveType.TILE_PUSH_RIGHT)); 

        // up push
        current = start; 
        while (board.isOnBoard(current) && board.tileAt(current) != null) current = current.up(); 
        if (isValidPush(board, start, current, playerTurn)) moves.Add(new Rules.ValidMove(current, Rules.MoveType.TILE_PUSH_UP)); 

        // down push
        current = start; 
        while (board.isOnBoard(current) && board.tileAt(current) != null) current = current.down(); 
        if (isValidPush(board, start, current, playerTurn)) moves.Add(new Rules.ValidMove(current, Rules.MoveType.TILE_PUSH_DOWN)); 

        return moves; 
    }
}