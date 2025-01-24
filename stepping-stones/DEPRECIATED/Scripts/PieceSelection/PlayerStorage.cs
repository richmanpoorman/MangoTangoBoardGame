using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerStorage : Node2D
{
	[Export]
	public Node2D selectedPiece {get; set;}
	// public List<Node2D> piecesAtTile;
	[Export]
	public Node2D board;
	// public List<Node2D> piecesAtTile;

	[Export]
	public Vector2I selectedLocation{get; set;}


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void onPieceSelected(InputEventMouse mouseEvent, Sprite2D piece) {
		//this.selectedLocation = ((TileMapLayer)board).LocalToMap(mouseEvent.Position);
		this.selectedPiece = piece;
		GD.Print("Huzzah piece selected");

	}
}
