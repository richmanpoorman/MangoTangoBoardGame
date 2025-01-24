
// Represents the logistic of the piece under the hood
public interface Piece
{
	// Wrappers for different fields of the piece
	public enum Color {
		PLAYER_1, 
		PLAYER_2
	};

	public enum PieceType {
		TILE, 
		SCOUT
	};

	// Functions 
	public Color color();
	public PieceType pieceType();

}
	

