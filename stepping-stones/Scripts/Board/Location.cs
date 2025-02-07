using Godot;
using System;

public class Location
{
	private int x;
	private int y;

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

	/*changeLocation
	Inputs: int newRow, int newColumn
	Returns: None
	Description: Changes internal variables to new provided location*/
	public void changeLocation(int newRow, int newColumn){
		x = newRow;
		y = newColumn;
	}
}
