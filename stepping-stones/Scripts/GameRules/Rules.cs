using System.Collections.Generic;

public interface Rules {
    public enum MoveType {
        TILE_PUSH_LEFT, TILE_PUSH_RIGHT, TILE_PUSH_UP, TILE_PUSH_DOWN, 
        SCOUT_MOVE, TILE_MOVE, 
        TILE_PLACE
    };

    public struct ValidMove {
        public ValidMove(Location _location, MoveType _move) {
            location = _location; 
            move     = _move;
        }
        public Location location; 
        public MoveType move; 
    }

    public bool isValidPush(Board board, Location from, Location to, Piece.Color playerTurn);
    public bool isValidTileMove(Board board, Location from, Location to, Piece.Color playerTurn);
    public bool isValidScoutMove(Board board, Location from, Location to, Piece.Color playerTurn);
    public bool isValidPlace(Board board, Location place, Piece.Color playerTurn); 

    public bool isValidPieceMove(Board board, Location from, Location to, Piece.Color playerTurn) {
        if (!board.isOnBoard(from) || !board.isOnBoard(to)) return false;
        if (board.tileAt(to) == null) return isValidTileMove(board, from, to, playerTurn); 
        else return isValidScoutMove(board, from, to, playerTurn); 
    }

    public IList<ValidMove> legalOptions(Board board, Location start, Piece.Color playerTurn); 

    public bool hasWon(Board board, Piece.Color playerTurn);
}