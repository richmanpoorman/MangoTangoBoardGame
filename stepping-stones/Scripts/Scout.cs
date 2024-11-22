using Godot;
using System;
using System.Collections.Generic;
//To Do: implement toString, and movement; improve checkLegalMove logic
public partial class Scout : Piece
{
	enum team{};
	private Team scoutTeam;
	private Location currentLocation;
	public Scout (Team team, Location startingLocation) {
		scoutTeam = team;
		currentLocation = startingLocation;
	}
	
	abstract String toString();
 	abstract List<Location> move(Board b, Location location); 
	
	abstract bool moveIsLegal(Board b, Location startingLocation, Location destination){
		//check if destination is within bounds. Note: add "check board dimensions to board class"
		
		if (b.getPiece(destination) != null) {
			return false;
		}
		
		int deltaX = Math.Abs(startingLocation.getX() - destination.getX());
		int deltaY = Math.Abs(startingLocation.getY() - destination.getY());
		
		return (deltaX == 1 || deltaY == 1);
	}
	
	Location getLocation() {
		return currentLocation;
	}
	
	Location setLocation(Location location) {
		currentLocation = location;
	}
}
