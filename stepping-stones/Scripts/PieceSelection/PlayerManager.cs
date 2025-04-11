using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerManager : Node
{

    private static Dictionary<PlayerColor, MoveSelector> playerSelectors; 
    public static void setPlayerMode(PlayerColor player, MoveSelector selector) { 
        selector.setPlayer(player);
        playerSelectors[player] = selector; 
    } 
}
