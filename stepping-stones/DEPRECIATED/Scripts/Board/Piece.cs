using Godot;
using System;
using System.Collections.Generic;

public interface Piece2
{
	enum team{};
 	abstract String toString();
 	abstract List<Location> move(Board2 b, Location location); //list of legal moves
	abstract bool moveIsLegal(Board2 b, Location from, Location to); //if move is legal
	Location getLocation();
	Location setLocation(Location location);
}
