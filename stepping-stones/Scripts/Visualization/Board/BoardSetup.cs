using Godot;
using System;

using System.Collections.Generic;


// TODO:: replace the piece types of type string with the enum for the piece types

[GlobalClass]
public partial class BoardSetup : Resource
{
    [ExportGroup("Board Dimensions")]
	[Export(PropertyHint.Range, "1,100,1,or_greater")]
	public int width { get; set; } = 5; 
	[Export(PropertyHint.Range, "1,100,1,or_greater")]
	public int height { get; set; } = 7; 
		
	[ExportGroup("Initial Board Setup")]
	[Export]
	public Godot.Collections.Array<PieceType> pieceTemplates { get; set; } = new Godot.Collections.Array<PieceType>(); 

    [Export]
    public Godot.Collections.Array<PieceInitialLocation> startPieces = new Godot.Collections.Array<PieceInitialLocation>();

	private List<Node>[,] boardData { get; set; }
    private Dictionary<string, PackedScene> pieceTypes = new Dictionary<string, PackedScene>();
	/*
	 * Constructors
	 */
	
	public BoardSetup() : this(5, 7) {}
	
	public BoardSetup(int _width, int _height) => _initializeBoard(new List<Node>[width, height]); 
	
	public BoardSetup(List<Node>[, ] _boardData) => _initializeBoard(_boardData);

	public void _initializeBoard(List<Node>[, ] _boardData) {
		width = _boardData.GetLength(0); 
		height = _boardData.GetLength(1);
		boardData = _boardData;

        // setPieces();
	}

    /*
     *  Functions
     */

    public void setPieces() {
        foreach (PieceType pieceType in pieceTemplates) {
            pieceTypes[pieceType.pieceName] = pieceType.template();
        }

        foreach (PieceInitialLocation piece in startPieces) {
            if (!pieceTypes.ContainsKey(piece.pieceType) || piece.column < 0 || piece.column >= width || piece.row < 0 || piece.column >= height)
                continue; 
            boardData[piece.column, piece.row].Add(pieceTypes[piece.pieceType].Instantiate());
        }
    }
    public List<Node>[, ] board() {
        return boardData;
    }

    public void setBoard(List<Node>[, ] _board) {
        boardData = _board;
    }
}
