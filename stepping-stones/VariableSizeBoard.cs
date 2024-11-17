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
	public void addPiece(Piece piece, Location location){
		///Need to add check to make sure space is empty
		board[location.getX(), location.getY()] = piece;
	}
	public void movePiece(Location from, Location to){
		Piece piece = board[from.getX(), from.getY()];
		board[to.getX(), to.getY()] = piece;
		board[from.getX(), from.getY()] = null;
	}
	public void pushPiece(Location pusher, Location firstPushee){
		///TODO
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
