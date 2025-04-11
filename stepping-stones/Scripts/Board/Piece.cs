
// Represents the logistic of the piece under the hood
public interface Piece
{
	// Wrappers for different fields of the piece
	

	// Functions 
	/*color
	Inputs: None
	Returns: enum Color
	Description: returns color/team of piece*/
	public PlayerColor color();
	/*pieceType
	Inputs: None
	Returns: enum PieceType
	Description: returns type of piece (eg tile, scout)*/
	public PieceType pieceType();

}
	

