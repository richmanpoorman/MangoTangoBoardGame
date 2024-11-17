using Godot;
using System;

using System.Collections.Generic;
using Godot.Collections;

public partial class BoardRenderer : TileMapLayer
{
	[Export]
	public BoardSetup initialBoard { get; set; } = new BoardSetup(5, 7); 

	private int width, height; 
	private List<Node2D>[, ] board = {};

	[Export]
	public Node2D parentNode;
	[Export] // TEST 
	public PackedScene redTile;
	[Export] // TEST 
	public PackedScene blueTile;
	[Export] // TEST 
	public PackedScene redScout;
	[Export] // TEST 
	public PackedScene blueScout; 

	private List<Node2D> pieces; 

	/*
	 *  Constructors
	 */

	public BoardRenderer() {}

	

	/*
	 * Functions
	 */
	
	private void setUpBoard() {
		initialBoard.setPieces();
		board = initialBoard.board();
		width = board.GetLength(0);
		height = board.GetLength(1);

		foreach (List<Node2D> cell in board)
			foreach (Node2D node in cell)
				AddChild(node);
	}
	private void placePiecesOnBoard() {
		int width  = board.GetLength(0);  
		int height = board.GetLength(1); 

		for (int c = 0; c < width; c++) for (int r = 0; r < height; r++) {
			foreach (Node2D node in board[c, r]) {
				parentNode.AddChild(node);
				node.Position = MapToLocal(new Godot.Vector2I(c, r));
			}
		}
	}


	private void placeDefinedPieces() { // TEST
		Node2D rTile = (Node2D)redTile.Instantiate(),
		       bTile = (Node2D)blueTile.Instantiate(),
			   rScout = (Node2D)redScout.Instantiate(),
			   bScout = (Node2D)blueScout.Instantiate();
	
		parentNode.AddChild(rTile);
		parentNode.AddChild(bTile);
		parentNode.AddChild(rScout);
		parentNode.AddChild(bScout);

		rTile.Position = MapToLocal(new Godot.Vector2I(-1, 0)); 
		rScout.Position = MapToLocal(new Vector2I(-1, 0)); 

		bTile.Position = MapToLocal(new Vector2I(1, 0)); 
		bScout.Position = MapToLocal(new Vector2I(1, 0));

		GD.Print("Defined Pieces");
	}

	/* 
	 * Runtime Runners
	 */
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// setUpBoard();
		// placePiecesOnBoard();
		placeDefinedPieces();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
