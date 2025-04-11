


public class Scout : Piece {
	
	private PlayerColor __color;

	public Scout(PlayerColor _color) {
		__color = _color; 
	}
	public PlayerColor color() { return __color; }
	public PieceType pieceType() { return PieceType.SCOUT; }
}
