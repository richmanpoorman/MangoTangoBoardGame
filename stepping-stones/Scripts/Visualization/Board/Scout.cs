


public class Scout : Piece {
	
	private Piece.Color __color;

	public Scout(Piece.Color _color) {
		__color = _color; 
	}
	public Piece.Color color() { return __color; }
	public Piece.PieceType pieceType() { return Piece.PieceType.SCOUT; }
}
