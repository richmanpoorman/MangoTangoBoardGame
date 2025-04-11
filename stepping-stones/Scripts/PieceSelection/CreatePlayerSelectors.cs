using Godot;
using System;
using System.Collections.Generic;  

public partial class CreatePlayerSelectors : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public enum PlayerType {
		LOCAL, ONLINE, AI
	};

	[Export] 
	private PackedScene localSelector, onlineSelector, aiSelector; 
	
	private Dictionary<Piece.Color, Node2D> playerSelectors; 


	public override void _Ready() {
		EventBus bus = EventBus.Bus; 
		bus.onPlayerJoin  += onPlayerJoin; 
		bus.onPlayerLeave += onPlayerLeave; 
	}


	private void onPlayerJoin(Piece.Color player, PlayerType selectorType) {
		// Get the right node
		Node2D? selector = selectorType switch {
			PlayerType.LOCAL  => localSelector  is null ? null : localSelector.Instantiate<Node2D>(), 
			PlayerType.ONLINE => onlineSelector is null ? null : onlineSelector.Instantiate<Node2D>(), 
			PlayerType.AI     => aiSelector     is null ? null : aiSelector.Instantiate<Node2D>(),
			_                 => null
		};

		// Set the player if valid, otherwise delete all work
		if (selector is null) 
			return; 
		else if (selector is MoveSelector select)
			select.setPlayer(player); 
		else {
			selector.QueueFree(); 
			return; 
		}

		// Set the player to the selector (deleting it if selected from before)
		if (playerSelectors.ContainsKey(player)) playerSelectors[player].QueueFree(); 
		playerSelectors[player] = selector; 
	}

	private void onPlayerLeave(Piece.Color player) {
		if (!playerSelectors.ContainsKey(player)) return; 
		if (playerSelectors[player] is not null) playerSelectors[player].QueueFree();
		playerSelectors.Remove(player); 
	}
}
