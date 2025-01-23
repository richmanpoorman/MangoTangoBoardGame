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
		///Check if valid move
		//todo: to is empty; from is 1 space away from to
		int dist = Math.Abs(to.getX()-from.getX()) + Math.Abs(to.getY()-from.getY());
		if(board[to.getX(), to.getY()] == null && dist == 1){
			Piece piece = board[from.getX(), from.getY()];
			board[to.getX(), to.getY()] = piece;
			board[from.getX(), from.getY()] = null;
		}
	}
	
	private bool isValidPush(Location pusher, Location firstPushee){
		return true; //TODO: IMPLEMENT
	}

	public void pushPiece(Location pusher, Location firstPushee){
		///TODO
		if(isValidPush(pusher, firstPushee)){
			int ydir = (firstPushee.getY() - pusher.getY())/Math.abs(firstPushee.getY() - pusher.getY());
			int xdir = (firstPushee.getX() - pusher.getX())/Math.abs((firstPushee.getX() - pusher.getX()));
			int x = firstPushee.getX();
			int y = firstPushee.getY();
			while(board[x+xdir, y+ydir] != null){}//MOVE TO last thing in row
		}
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
