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
		_eventBus.onChangePieceTileset += _onTilesetChange;
		// Connect(EventBus.SignalName.onBoardUpdate, Callable.From(_onUpdate));
		// Connect(EventBus.SignalName.onBoardReset, Callable.From(onRestart));
		_onTilesetChange(SceneManager.Instance.playerTiles, SceneManager.Instance.tilesetIDs);
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
		_onUpdate(); 
	}

	public void _onTilesetChange(TileSet newSprites, Godot.Collections.Dictionary<Piece.Color, Godot.Collections.Dictionary<Piece.PieceType, int>> tilesetIDs) {
		spacesLayer.TileSet = newSprites;
		tileLayer.TileSet   = newSprites; 
		scoutLayer.TileSet  = newSprites; 
		
		spaceTileID    = tilesetIDs[Piece.Color.BLANK][Piece.PieceType.BLANK];
		player1ScoutID = tilesetIDs[Piece.Color.PLAYER_1][Piece.PieceType.SCOUT];
		player2ScoutID = tilesetIDs[Piece.Color.PLAYER_2][Piece.PieceType.SCOUT];
		player1TileID  = tilesetIDs[Piece.Color.PLAYER_1][Piece.PieceType.TILE];
		player2TileID  = tilesetIDs[Piece.Color.PLAYER_2][Piece.PieceType.TILE];

		_onRestart(); 
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
