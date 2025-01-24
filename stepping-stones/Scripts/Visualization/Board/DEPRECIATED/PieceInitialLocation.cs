using Godot;
using System;

using Godot.Collections;


// TODO:: replace the piece types of type string with the enum for the piece types

[GlobalClass]
public partial class PieceInitialLocation : Resource
{
    [Export]
    public string pieceType { get; set; }

    [Export(PropertyHint.Range, "0,100,1,or_greater")]
    public int column { get; set; }

    [Export(PropertyHint.Range, "0,100,1,or_greater")]
    public int row { get; set; }

    public PieceInitialLocation() : this("", -1, -1) {} 

    public PieceInitialLocation(string _pieceType, int _column, int _row) {
        pieceType = _pieceType; 
        column    = _column; 
        row       = _row; 
    }

}
