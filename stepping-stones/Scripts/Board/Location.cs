using Godot;
using System;
using System.Collections.Generic;
using System.Data;

public class Location
{
	private int x;
	private int y;

	private static Dictionary<Tuple<int, int>, Location> locations = new Dictionary<Tuple<int, int>, Location>(); 

	public static Location at(int row, int column) {
		Tuple<int, int> position = new Tuple<int, int>(row, column); 
		if (!locations.ContainsKey(position)) locations[position] = new Location(row, column); 
		return locations[position]; 
	}

	/*Location (constructor)
	Inputs: int row, int column
	Returns: Location
	Description: Constructor of Location*/
	public Location(int row, int column){
		x = row;
		y = column;
	}
	
	/*row
	Inputs: None
	Returns: Int
	Description: Returns row of Location instance*/
	public int row(){
		return x;
	}
	/*column
	Inputs: None
	Returns: Int
	Description: Returns column of Location instance*/
	public int column(){
		return y;
	}


	public Location left() { return new Location(x, y - 1); }
	public Location right() { return new Location(x, y + 1); }
	public Location up() { return new Location(x - 1, y); }
	public Location down() { return new Location(x + 1, y); }
}
