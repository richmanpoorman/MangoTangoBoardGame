using Godot;
using System;

public partial class GameLogic : Node
{
	private VariableSizeBoard board;
	

	
	public bool isValidPush(Location pusher, Location firstEmpty){

		return true; //TODO: IMPLEMENT
	}

	public bool isValidPlacement(Location location){
		return board.getPiece(location)==null;
	}

	public bool isValidTileMove(Location from, Location to){
		int dist = Math.Abs(to.row()-from.row()) + Math.Abs(to.column()-from.column());
		return board.getPiece(to) == null && dist == 1;
	}

	public bool isValidScout(){
		return true;//TODO: IMPLEMENT
	}

	public bool isLoseGamestate(){
		return false; //TODO: IMPLEMENT
	}

	public void getValidMoves(){//need to make a struct to have position, move type
		//TODO: IMPLEMENT
	}

	public void movePiece(Location from, Location to){
		///Check if valid move
		//todo: to is empty; from is 1 space away from to
		int dist = Math.Abs(to.row()-from.row()) + Math.Abs(to.column()-from.column());
		if(isValidTileMove(from, to)){
			Piece piece = board.getPiece(from);
			board.addPiece(piece, to);
			board.emptySpace(from);
		}
	}

	public void pushPiece(Location pusher, Location firstPushee){
		//instead of first pushee, first empty space
		///TODO
		if(isValidPush(pusher, firstPushee)){
			int ydir = (firstPushee.column() - pusher.column())/Math.Abs(firstPushee.column() - pusher.column());
			int xdir = (firstPushee.row() - pusher.row())/Math.Abs((firstPushee.row() - pusher.row()));
			int x = firstPushee.row();
			int y = firstPushee.column();
			while(board.getPiece(x, y) != null){//MOVE TO space after last thing in row
				x += xdir;
				y += ydir;
			}
			while(x != pusher.row() || y != pusher.column()){
				Location from = new Location(x-xdir, y-ydir);
				Location to = new Location (x, y);
				movePiece(from, to);
				x-=xdir;
				y-=ydir;

			}

		}
	}
}
