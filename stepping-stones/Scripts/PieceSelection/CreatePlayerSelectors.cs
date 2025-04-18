using Godot;
using System;
using System.Collections.Generic;  

public partial class CreatePlayerSelectors : Node2D
{
	// Called when the node enters the scene tree for the first time.
	

	[Export] 
	private PackedScene localSelector, onlineSelector, aiSelector; 
	
	private Dictionary<PlayerColor, Node2D> playerSelectors = new Dictionary<PlayerColor, Node2D>(); 


	public override void _Ready() {
		EventBus bus = EventBus.Bus; 
		bus.onPlayerJoin  += onPlayerJoin; 
		bus.onPlayerLeave += onPlayerLeave; 

		// Add a player for each type
		foreach (var (player, playerType) in PlayerManager.players) 
			onPlayerJoin(player, playerType);
		
	}

    public override void _EnterTree()
    {
		EventBus bus = EventBus.Bus; 
        bus.onPlayerJoin  -= onPlayerJoin; 
		bus.onPlayerLeave -= onPlayerLeave; 
    }



	private void onPlayerJoin(PlayerColor player, PlayerType selectorType) {
		// Get the right node
		Node2D? selector = selectorType switch {
			PlayerType.LOCAL  => localSelector  is null ? null : localSelector.Instantiate<Node2D>(), 
			PlayerType.ONLINE => onlineSelector is null ? null : onlineSelector.Instantiate<Node2D>(), 
			PlayerType.AI     => aiSelector     is null ? null : aiSelector.Instantiate<Node2D>(),
			_                 => null
		};

		// Set the player if valid, otherwise delete all work
		if (selector is null) {
			GD.PrintErr("The piece selector can't be instantiated"); 
			return; 
		} else if (selector is MoveSelector select) {
			select.setPlayer(player); 
			PlayerManager.setPlayerMode(player, selectorType);
			GD.Print($"{ selectorType switch { PlayerType.LOCAL => "Local", PlayerType.ONLINE => "Online", PlayerType.AI => "AI", _ => "???"} } Selector was instantiated for Player {player switch { PlayerColor.PLAYER_1 => "1", PlayerColor.PLAYER_2 => "2", _ => "???"} }");
		} else {
			GD.PrintErr("The piece selector is NOT a move selector");
			selector.QueueFree(); 
			return; 
		}
		// Set the player to the selector (deleting it if selected from before)
		if (playerSelectors.ContainsKey(player)) playerSelectors[player].QueueFree(); 
		playerSelectors[player] = selector;

		this.AddChild(selector);
	}

	private void onPlayerLeave(PlayerColor player) {
		if (!playerSelectors.ContainsKey(player)) return; 
		if (playerSelectors[player] is not null) playerSelectors[player].QueueFree();
		playerSelectors.Remove(player); 
		PlayerManager.removePlayer(player);
	}
}
