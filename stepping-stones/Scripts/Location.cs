using Godot;
using System;

public partial class Location : Node
{
	private int row;
	private int col;
	public Location(int _row, int _col){
		row = _row;
		col = _col;
	}
	
	public int get_row(){
		return row;
	}
	public int get_col(){
		return col;
	}

	public void changeLoc(int new_row, int new_col){
		row = new_row;
		col = new_col;
	}

	public string toString(){
		return "("+ row.ToString() + ", " + col.ToString() + ")";
	}
}
