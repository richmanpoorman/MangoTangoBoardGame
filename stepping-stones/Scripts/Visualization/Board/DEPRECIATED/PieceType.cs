using Godot;
using System;

[GlobalClass]
public partial class PieceType : Resource
{
    [Export]
    public string pieceName { get; set; }

    [Export]
    public PackedScene pieceTemplate { get; set; }

    public PieceType() {
        pieceName     = ""; 
        pieceTemplate = null; 
    } 

    public PieceType(string _pieceName, PackedScene _pieceTemplate) {
        pieceName     = _pieceName; 
        pieceTemplate = _pieceTemplate;
    }

    public PieceType(string _pieceName, string _templateFile) {
        pieceName     = _pieceName; 
        pieceTemplate = GD.Load<PackedScene>(_templateFile);
    }

    public PackedScene template() {
        return pieceTemplate;
    }
}
