using Godot;
using System;
using System.Collections.Generic;
//To Do: implement toString, and movement; improve checkLegalMove logic
public partial class Scout : Piece
{
	private Piece.team scoutTeam;
	private Location currentLocation;

	public Scout(Piece.team team, Location startingLocation) {
		scoutTeam = team;
		currentLocation = startingLocation;
	}
    public Location getLocation()
    {
        return currentLocation;
    }

    public List<Location> move(Board b, Location location)
    {
        List<Location> legalMoves = new List<Location>();

		return legalMoves;
    }

    public bool moveIsLegal(Board b, Location from, Location to)
    {
		//check if destination is within bounds
        //make sure that destination is one space away from source
		int deltaX = Math.Abs(to.getX() - from.getX());
		int deltaY = Math.Abs(to.getY() - from.getY());
		if (deltaX > 1 || deltaY > 1) {
			return false;
		}

		//check if space is already occupied
		if (b.getPiece(to) != null) {
			return false;
		}

		return true;
    }

    public Location setLocation(Location location)
    {
        if (location != null) {
			currentLocation = location;
			return currentLocation;
		}
		else {
			throw new ArgumentException("Invalid location");
		}
    }

    public string toString()
    {
        return $"{scoutTeam} Scout at {currentLocation}";
    }
}