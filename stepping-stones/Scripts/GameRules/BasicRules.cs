using System;
using System.Collections.Generic;
using Godot;

public class BasicRules : Rules
{

    public BasicRules() {}

    public bool hasWon(Board board, Piece.Color playerTurn)
    {
        Piece.Color otherColor = otherPlayer(playerTurn); 

        Scout?[,] scouts = board.scoutLayer(); 
        int[] size = board.size(); 
        for (int r = 0; r < size[0]; r++) {
            for (int c = 0; c < size[1]; c++) {
                if (scouts[r, c] == null) continue; 
                if (scouts[r, c].color() == otherColor) return false;
            }
        }
        return true; 
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

            if (addingCurrentPlayer) tileSum += 1; 
            else tileSum -= 1; 
        }

        if (addingCurrentPlayer) return false; // Need to push at least one opponent tile 
        if (tileSum != 1) return false; // Need to have one more player tile then opponent tile
        
        return true;
    }
    public bool isValidTileMove(Board board, Location from, Location to, Piece.Color playerTurn) {
        if (!board.isOnBoard(from) || !board.isOnBoard(to)) return false;
        bool isOrthogonallyConnected = Math.Abs(from.row() - to.row()) + Math.Abs(from.column() - to.column()) == 1; 
        if (!isOrthogonallyConnected) return false; 
        Tile? tile = board.tileAt(from); 
        if (tile == null) return false; 
        if (tile.color() != playerTurn) return false; 
        if (board.tileAt(to) != null) return false;
        
        return true;
    }
    public bool isValidScoutMove(Board board, Location from, Location to, Piece.Color playerTurn) {
        if (!board.isOnBoard(from) || !board.isOnBoard(to)) return false;
        bool isOrthogonallyConnected = Math.Abs(from.row() - to.row()) + Math.Abs(from.column() - to.column()) == 1; 
        if (!isOrthogonallyConnected) return false; 
        Scout? scout = board.scoutAt(from); 
        if (scout == null) return false; 
        if (scout.color() != playerTurn) return false; 
        Tile? tile = board.tileAt(to);
        if (tile == null) return false; 
        if (tile.color() != playerTurn) return false; 
        if (board.scoutAt(to) != null) return false;
        return true; 
        
    }

    public bool isValidPlace(Board board, Location place, Piece.Color playerTurn)
    {
        return board.tileAt(place) == null;
    }

    public IList<Rules.ValidMove> legalOptions(Board board, Location start, Piece.Color playerTurn)
    {
        throw new System.NotImplementedException();
    }

    private Piece.Color otherPlayer(Piece.Color playerTurn) {
        return playerTurn == Piece.Color.PLAYER_1 ? Piece.Color.PLAYER_2 : Piece.Color.PLAYER_1;
    }
}