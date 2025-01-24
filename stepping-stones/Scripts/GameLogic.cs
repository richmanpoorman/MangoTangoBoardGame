using Godot;
using System;

public partial class GameLogic : Node
{
	private VariableSizeBoard board;
	
	public void movePiece(Location from, Location to){
		///Check if valid move
		//todo: to is empty; from is 1 space away from to
		int dist = Math.Abs(to.getX()-from.getX()) + Math.Abs(to.getY()-from.getY());
		if(board.getPiece(to) == null && dist == 1){
			Piece piece = board.getPiece(from);
			board.addPiece(piece, to);
			board.emptySpace(from);
		}
	}

	
	public bool isValidPush(Location pusher, Location firstEmpty){

		return true; //TODO: IMPLEMENT
	}

	public bool isValidPlacement(){
		//true
		return true;//TODO: IMPLEMENT
	}

	public bool isValidTileMove(){
		return true;//TODO: IMPLEMENT
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


	public void pushPiece(Location pusher, Location firstPushee){
		//instead of first pushee, first empty space
		///TODO
		if(isValidPush(pusher, firstPushee)){
			int ydir = (firstPushee.getY() - pusher.getY())/Math.Abs(firstPushee.getY() - pusher.getY());
			int xdir = (firstPushee.getX() - pusher.getX())/Math.Abs((firstPushee.getX() - pusher.getX()));
			int x = firstPushee.getX();
			int y = firstPushee.getY();
			while(board.getPiece(x, y) != null){//MOVE TO space after last thing in row
				x += xdir;
				y += ydir;
			}
			while(x != pusher.getX() || y != pusher.getY()){
				Location from = new Location(x-xdir, y-ydir);
				Location to = new Location (x, y);
				movePiece(from, to);
				x-=xdir;
				y-=ydir;

			}

		}
	}
}
