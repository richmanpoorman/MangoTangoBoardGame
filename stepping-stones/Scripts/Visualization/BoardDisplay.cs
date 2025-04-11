using Godot;
using System;
using System.Diagnostics;

public partial class BoardDisplay : Node2D
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	private BoardManager boardManager; 

	[Export]
	private TileMapLayer spacesLayer, tileLayer, scoutLayer; 

	[Export]
	private int spaceTileID, player1ScoutID, player2ScoutID, player1TileID, player2TileID; 
	
	private EventBus _eventBus; 
	
	private Board board; 
	
	public override void _Ready()
	{
		_eventBus = EventBus.Bus;

		_eventBus.onBoardUpdate += _onUpdate; 
		_eventBus.onBoardReset  += _onRestart; 
		// Connect(EventBus.SignalName.onBoardUpdate, Callable.From(_onUpdate));
		// Connect(EventBus.SignalName.onBoardReset, Callable.From(onRestart));
		initializeBoard(); 
		updateDisplay(); 
	}

	public void initializeBoard() {
		
		// Position = new Vector2(Position.X - 60, Position.Y - 20);
		board = boardManager.board(); 

		int[] size = board.size(); 
		spacesLayer.Clear(); 
		for (int row = 0; row < size[0]; row++) {
			for (int column = 0; column < size[1]; column++) {
				spacesLayer.SetCell(new Vector2I(column, row), spaceTileID, Vector2I.Zero);
			}
		}
	}

	#nullable enable
	public void _onUpdate() {
		updateDisplay(); 
	}

	public void _onRestart() {
		initializeBoard();
	}
	public (Vector2, Vector2I) getSizingInfo() {
		return (spacesLayer.Position, 
				spacesLayer.GetUsedRect().Size * spacesLayer.TileSet.TileSize);
	}
	public void updateDisplay() {
		board = boardManager.board(); 
		
		Tile?[,]  tiles  = board.tileLayer(); 
		Scout?[,] scouts = board.scoutLayer(); 

		// GD.Print("Tile Size: ", tiles.GetLength(0), ", ", tiles.GetLength(1));
		// GD.Print("Scout Size: ", scouts.GetLength(0), ", ", scouts.GetLength(1));
		tileLayer.Clear();
		for (int row = 0; row < tiles.GetLength(0); row++) {
			for (int column = 0; column < tiles.GetLength(1); column++) {
				Tile? tile = tiles[row, column]; 
				
				if (tile == null) continue; 
				// GD.Print("Tile: ", row, ", ", column);
				_setProperTile(tile, new Vector2I(column, row));
			}
		}
		
		scoutLayer.Clear();
		for (int row = 0; row < scouts.GetLength(0); row++) {
			for (int column = 0; column < scouts.GetLength(1); column++) {
				Scout? scout = scouts[row, column]; 

				if (scout == null) continue; 
				// GD.Print("Scout: ", row, ", ", column);
				_setProperScout(scout, new Vector2I(column, row));
			}
		}
	}
	#nullable disable


	private void _setProperTile(Tile tile, Vector2I position) {
		switch(tile.color()) {
			case Piece.Color.PLAYER_1:
				tileLayer.SetCell(position, player1TileID, Vector2I.Zero);
			break; 
			case Piece.Color.PLAYER_2: 
				tileLayer.SetCell(position, player2TileID, Vector2I.Zero);
			break; 
			default:
				GD.Print("Tile has no color"); 
			break; 
		}
	}
	private void _setProperScout(Scout scout, Vector2I position) {
		switch(scout.color()) {
			case Piece.Color.PLAYER_1:
				scoutLayer.SetCell(position, player1ScoutID, Vector2I.Zero);
			break; 
			case Piece.Color.PLAYER_2: 
				scoutLayer.SetCell(position, player2ScoutID, Vector2I.Zero);
			break; 
			default:
				GD.Print("Scout has no color"); 
			break; 
		}
	}
}
