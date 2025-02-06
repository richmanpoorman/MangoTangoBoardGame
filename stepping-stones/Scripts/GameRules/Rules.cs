using System.Collections.Generic;

public interface Rules {
    public enum MoveType {
        TILE_PUSH_LEFT, TILE_PUSH_RIGHT, TILE_PUSH_UP, TILE_PUSH_DOWN, 
        SCOUT_MOVE, TILE_MOVE, 
        TILE_PLACE
    };

    public struct ValidMove {
        public Location location; 
        public MoveType move; 
    }

    public bool isValidPush(Board board, Location from, Location to, Piece.Color playerTurn);
    public bool isValidTileMove(Board board, Location from, Location to, Piece.Color playerTurn);
    public bool isValidScoutMove(Board board, Location from, Location to, Piece.Color playerTurn);
    public bool isValidPlace(Board board, Location place, Piece.Color playerTurn); 

    public IList<ValidMove> legalOptions(Board board, Location start, Piece.Color playerTurn); 

    public bool hasWon(Board board, Piece.Color playerTurn);
}