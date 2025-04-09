using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerManager : Node
{

    private static Dictionary<Piece.Color, MoveSelector> playerSelectors; 
    public static void setPlayerMode(Piece.Color player, MoveSelector selector) { 
        selector.setPlayer(player);
        playerSelectors[player] = selector; 
    } 
}
