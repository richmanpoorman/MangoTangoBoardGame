
using System;
using System.Data;
using Godot;
using Microsoft.CSharp.RuntimeBinder;

public class GridSteppingStonesBoard : SteppingStonesBoard
{
    #nullable enable
    private Board _board; 

    public GridSteppingStonesBoard(int width, int height) { _board = new GridBoard(width, height); }
    public GridSteppingStonesBoard(Board board) { _board = board; }
    public bool placeTile(Tile tile, Board.Position position)
    {
        if (!validPositionOnBoard(position)) return false; 
        if (_board.tileAt(position) != null) return false;
        _board.addTile(tile, position); 
        return true;
    }

    public Board board() { return _board; }

    public bool movePiece(Board.Position from, Board.Position to) {
        if (!validPositionOnBoard(from) || !validPositionOnBoard(to)) return false; 
        if (from.row == to.row && from.column == to.column) return false;
        if (_board.tileAt(from) == null) return false; 
        
        bool movedAtLeastOne = false; 

        if (_board.tileAt(to) == null) {
            movedAtLeastOne = true;
            _board.moveTile(from, to);
        }
        
        if (_board.tileAt(to) != null && _board.scoutAt(from) != null) {
            if (_board.scoutAt(to) == null) {
                movedAtLeastOne = true;
                _board.moveScout(from, to); 
            }
            else return false;
        } 
        
        return movedAtLeastOne; 
    }

    public bool pushMove(Board.Position start, Board.Position to) {
        if (!validPositionOnBoard(start)) return false;
        if (start.row != to.row && start.column != to.column) return false;
        if (start.row == to.row && start.column == to.column) return false;

        Vector2I direction = new Vector2I(Math.Sign(to.row - start.row), Math.Sign(to.column - start.column)),
                 first     = new Vector2I(start.row, start.column), 
                 last      = new Vector2I(to.row, to.column);
        for (Vector2I moveTo = last, moveFrom = last - direction; moveTo != first; moveTo = moveFrom, moveFrom -= direction) {
            GD.Print("Move ", moveFrom, " to ", moveTo);
            Board.Position fromPosition = new Board.Position(moveFrom.X, moveFrom.Y),
                           toPosition   = new Board.Position(moveTo.X, moveTo.Y);

            // If the start position is not valid, then don't do anything
            if (!validPositionOnBoard(fromPosition)) continue; 
            
            // If the position to move the tile to is not valid, assume pushing off board
            if (!validPositionOnBoard(toPosition)) {
                if (_board.tileAt(fromPosition) is Tile) _board.removeTile(fromPosition);
                if (_board.scoutAt(fromPosition) is Scout) _board.removeScout(fromPosition);
                continue; 
            }

            // Otherwise, assume that the push is valid, and move all of the stuff
            if (_board.tileAt(fromPosition) is Tile) _board.moveTile(fromPosition, toPosition);
            if (_board.scoutAt(fromPosition) is Scout) _board.moveScout(fromPosition, toPosition);
        }

        return true;
    }

    public bool pushDirection(Board.Position start, SteppingStonesBoard.PushDirection direction)
    {
        if (!validPositionOnBoard(start)) return false;


        Vector2I pushTo;
        switch(direction) {
            case SteppingStonesBoard.PushDirection.UP:
                pushTo = Vector2I.Down; 
            break; 
            case SteppingStonesBoard.PushDirection.DOWN: 
                pushTo = Vector2I.Up; 
            break; 
            case SteppingStonesBoard.PushDirection.LEFT: 
                pushTo = Vector2I.Left; 
            break; 
            case SteppingStonesBoard.PushDirection.RIGHT: 
                pushTo = Vector2I.Right; 
            break; 
            default: 
                pushTo = Vector2I.Zero; 
            break; 
        }

        int[] size = _board.size();
        Vector2I position = new Vector2I(start.row, start.column); 
        while (position.X < 0 || position.X >= size[0] || position.Y < 0 || position.Y >= size[1]) {
            Board.Position boardPosition = new Board.Position(position.X, position.Y);
            if (_board.tileAt(boardPosition) == null) return pushMove(start, boardPosition); 
            position += pushTo;
        }

        return pushMove(start, new Board.Position(position.X, position.Y));
    }

    private bool validPositionOnBoard(Board.Position position) {
        int[] size = _board.size(); 
        return position.row >= 0 && position.row < size[0] && position.column >= 0 && position.column < size[1]; 
    }

    public int[] size() { return _board.size(); }

    public Tile[,] tileLayer() { return _board.tileLayer(); }

    public Scout[,] scoutLayer() { return _board.scoutLayer(); }

    public Tile? tileAt(Board.Position position) { return _board.tileAt(position);  }

    public Scout? scoutAt(Board.Position position) { return _board.scoutAt(position); }

    public void addTile(Tile tile, Board.Position position) { _board.addTile(tile, position); }

    public Tile? removeTile(Board.Position position) { return _board.removeTile(position); }

    public void moveTile(Board.Position from, Board.Position to) { _board.moveTile(from, to); }

    public void addScout(Scout scout, Board.Position position) { _board.addScout(scout, position); }

    public Scout? removeScout(Board.Position position) { return _board.removeScout(position); }

    public void moveScout(Board.Position from, Board.Position to) { _board.moveScout(from, to); }
    #nullable disable
}