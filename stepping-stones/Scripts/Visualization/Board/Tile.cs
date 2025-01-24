
public class Tile : Piece {
	
	private Piece.Color __color;

	public Tile(Piece.Color _color) {
		__color = _color; 
	}
	public Piece.Color color() { return __color; }
	public Piece.PieceType pieceType() { return Piece.PieceType.TILE; }
}
