using Godot;
using System;

public partial class Location : Node
{
	private int x;
	private int y;
	public Location(int X, int Y){
		x = X;
		y = Y;
	}
	
	public int getX(){
		return x;
	}
	public int getY(){
		return y;
	}

	public void changeLoc(int newX, int newY){
		x = newX;
		y = newY;
	}
}
