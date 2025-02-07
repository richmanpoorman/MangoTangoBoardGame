using Godot;
using System;
using System.Collections.Generic;

public partial class DisplayOptions : Node
{
	[Export]
	private TileMapLayer selectionLayer; 
	[Export]
	private int pushUpID, pushDownID, pushLeftID, pushRightID, scoutMoveID, tileMoveID; 
	public void clear() { selectionLayer.Clear(); }

	public void updateOptions(IList<Rules.ValidMove> moveOptions) {
		foreach (Rules.ValidMove option in moveOptions) 
			displaySingleOption(option); 
	}

	private void displaySingleOption(Rules.ValidMove option) {
		Location location = option.location; 
		Vector2I position = new Vector2I(location.column(), location.row()); 
		switch(option.move) {
			case Rules.MoveType.TILE_PUSH_LEFT: 
				selectionLayer.SetCell(position, pushLeftID, Vector2I.Zero);
			break; 
			case Rules.MoveType.TILE_PUSH_RIGHT:
				selectionLayer.SetCell(position, pushRightID, Vector2I.Zero);
			break; 
			case Rules.MoveType.TILE_PUSH_UP: 
				selectionLayer.SetCell(position, pushUpID, Vector2I.Zero);
			break; 
			case Rules.MoveType.TILE_PUSH_DOWN:
				selectionLayer.SetCell(position, pushDownID, Vector2I.Zero);
			break; 
			case Rules.MoveType.SCOUT_MOVE: 
				selectionLayer.SetCell(position, scoutMoveID, Vector2I.Zero);
			break; 
			case Rules.MoveType.TILE_MOVE: 
				selectionLayer.SetCell(position, tileMoveID, Vector2I.Zero);
			break; 
			default: 
				GD.Print("There is an unknown option given");
			break; 
		}
	}
}
