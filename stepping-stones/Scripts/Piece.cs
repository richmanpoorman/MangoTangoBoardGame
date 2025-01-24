using Godot;
using System;
using System.Collections.Generic;

public interface Piece
{
	enum team{};
 	abstract String toString();
 	abstract List<Location> move(Board b, Location location); //list of legal moves
	abstract bool moveIsLegal(Board b, Location from, Location to); //if move is legal
	Location getLocation();
	Location setLocation(Location location);
}
