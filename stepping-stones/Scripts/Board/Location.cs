using Godot;
using System;

public class Location
{
	private int x;
	private int y;
	public Location(int row, int column){
		x = row;
		y = column;
	}
	
	public int row(){
		return x;
	}
	public int column(){
		return y;
	}

	public void changeLocation(int newRow, int newColumn){
		x = newRow;
		y = newColumn;
	}
}
