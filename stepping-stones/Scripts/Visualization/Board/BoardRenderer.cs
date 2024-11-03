using Godot;
using System;

using System.Collections.Generic;

public partial class BoardRenderer : TileMap
{
	[Export]
	public BoardSetup initialBoard { get; set; } = new BoardSetup(5, 7); 

	private List<Node>[, ] board;

	/*
	 *  Constructors
	 */

	public BoardRenderer() {}

	/*
	 * Functions
	 */
	
	


	/* 
	 * Runtime Runners
	 */
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		initialBoard.setPieces();
		board = initialBoard.board();
		Console.WriteLine("Ready!");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
