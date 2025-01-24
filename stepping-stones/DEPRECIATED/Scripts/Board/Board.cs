using Godot;
using System;

public interface Board2
{
	Piece getPiece(Location location);
	void addPiece(Piece piece, Location location);
	void movePiece(Location from, Location to);
	void pushPiece(Location pusher, Location firstPushee);
	Piece[,] getAllPieces();
 	void clear();
	
}
