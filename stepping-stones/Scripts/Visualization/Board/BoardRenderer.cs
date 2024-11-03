using Godot;
using System;

using System.Collections.Generic;
using Godot.Collections;

public partial class BoardRenderer : TileMapLayer
{
	[Export]
	public BoardSetup initialBoard { get; set; } = new BoardSetup(5, 7); 

	private List<Node2D>[, ] board;

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
		foreach (List<Node2D> cell in board)
			foreach (Node2D node in cell)
				AddChild(node);
	}
	private void placePiecesOnBoard() {
		int width  = board.GetLength(0);  
		int height = board.GetLength(1); 

		for (int c = 0; c < width; c++) for (int r = 0; r < height; r++) 
			foreach (Node2D node in board[c, r]) 
				node.Position = MapToLocal(new Godot.Vector2I(c, r));
	}


	/* 
	 * Runtime Runners
	 */
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		setUpBoard();
		placePiecesOnBoard();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
