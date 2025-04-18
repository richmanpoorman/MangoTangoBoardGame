using Godot;
using System;
using System.Collections.Generic;

public class PlayerManager
{

    public static Dictionary<PlayerColor, PlayerType> players { private set; get; } = new Dictionary<PlayerColor, PlayerType>();  
    public static void setPlayerMode(PlayerColor player, PlayerType selector) { 
        players[player] = selector; 
    } 

    public static void removePlayer(PlayerColor player) {
        if (!players.ContainsKey(player)) return; 
        players.Remove(player); 
    }

    public static void reset() {
        players.Clear();
    }
}
