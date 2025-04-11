
public class Tile : Piece {
	
	private PlayerColor __color;

	public Tile(PlayerColor _color) {
		__color = _color; 
	}
	public PlayerColor color() { return __color; }
	public PieceType pieceType() { return PieceType.TILE; }
}
