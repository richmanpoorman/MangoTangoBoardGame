using Godot;
using System;

public partial class GameLogic : Node
{
	private Board board;
	

	
	public bool isValidPush(Location pusher, Location firstEmpty){
		if(pusher.get_row()-firstEmpty.get_row() != 0 && pusher.get_col()-firstEmpty.get_col() != 0)
			return false;// if both aren't 0, then it's not in one direction-- bad!
		int colDir = (firstEmpty.get_col() - pusher.get_col())/Math.Max(Math.Abs(firstEmpty.get_col() - pusher.get_col()), 1);
	 	int rowDir = (firstEmpty.get_row() - pusher.get_row())/Math.Max(Math.Abs(firstEmpty.get_row() - pusher.get_row()), 1);
		int numTiles = Math.Max(Math.Abs(pusher.get_row()-firstEmpty.get_row()), Math.Abs(pusher.get_col()-firstEmpty.get_col()));

		Piece.Color team1 = board.tileAt(pusher).color();
		int numTeam1 = 0;
		int numTeam2 = 0;
		int row = pusher.get_row();
		int col = pusher.get_col();
		Location loc = new Location(row, col);
		bool countingT1=true;
		for (int i = 0; i < numTiles; i++){
			loc.changeLoc(row, col);
			Piece currPiece = board.tileAt(loc);
			if(currPiece == null){
				return false; //can't have gap in board
			} else if (currPiece.color() == team1) {
				
			}
			
			// TODO: ACCOUNT FOR SCOUT!
		}

		return true; //TODO: IMPLEMENT
	}

	public bool isValidPlacement(Location location){
		return board.tileAt(location)==null;
	}

	public bool isValidTileMove(Location from, Location to){
		int dist = Math.Abs(to.get_row()-from.get_row()) + Math.Abs(to.get_col()-from.get_col());
		return board.tileAt(to) == null && dist == 1;
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

	// public void movePiece(Location from, Location to){
	// 	///Check if valid move
	// 	//todo: to is empty; from is 1 space away from to
	// 	int dist = Math.Abs(to.get_row()-from.get_row()) + Math.Abs(to.get_col()-from.get_col());
	// 	if(isValidTileMove(from, to)){
	// 		Piece piece = board.getPiece(from);
	// 		board.addPiece(piece, to);
	// 		board.emptySpace(from);
	// 	}
	// }

	// public void pushPiece(Location pusher, Location firstPushee){
	// 	//instead of first pushee, first empty space
	// 	///TODO
	// 	if(isValidPush(pusher, firstPushee)){
	// 		int ydir = (firstPushee.get_col() - pusher.get_col())/Math.Abs(firstPushee.get_col() - pusher.get_col());
	// 		int xdir = (firstPushee.get_row() - pusher.get_row())/Math.Abs((firstPushee.get_row() - pusher.get_row()));
	// 		int x = firstPushee.get_row();
	// 		int y = firstPushee.get_col();
	// 		while(board.getPiece(x, y) != null){//MOVE TO space after last thing in row
	// 			x += xdir;
	// 			y += ydir;
	// 		}
	// 		while(x != pusher.get_row() || y != pusher.get_col()){
	// 			Location from = new Location(x-xdir, y-ydir);
	// 			Location to = new Location (x, y);
	// 			movePiece(from, to);
	// 			x-=xdir;
	// 			y-=ydir;

	// 		}

	// 	}
	// }
}
