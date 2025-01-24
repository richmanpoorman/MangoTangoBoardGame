using Godot;
using System;

public partial class VariableSizeBoard : Board
{
	private Piece[,] board;
	private int width;
	private int height;
	public VariableSizeBoard(int w, int h){
		width = w;
		height = h;
		board = new Piece[width,height];
	}
	public Piece getPiece(Location location){
		return board[location.getX(), location.getY()];
	}

	public Piece getPiece(int x, int y){
		return board[x, y];
	}

	public void addPiece(Piece piece, Location location){ //More of a "set piece" function
		board[location.getX(), location.getY()] = piece;
	}

	public void emptySpace(Location location){
		board[location.getX(), location.getY()] = null;
	}

	public void movePiece(Location from, Location to){

	}

	public void pushPiece(Location pusher, Location firstPushee){

	}
	
	public Piece[,] getAllPieces(){
		return board;
	}
 	public void clear(){
		for (int r = 0; r< width; r++){
			for (int c = 0; c < height; c++){
				board[r,c] = null;
			}
		}
	}
}
