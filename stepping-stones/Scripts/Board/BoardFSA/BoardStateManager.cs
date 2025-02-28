

using System.Collections.Generic;

public class BoardStateManager : BoardStateMachine
{
    private BoardState _currentState; 
    private SteppingStonesBoard _board; 
    private Rules _rules;
    public BoardStateManager(SteppingStonesBoard board, Rules rules, Dictionary<Piece.Color, int> tileCounts, Piece.Color startingColor) {
        changeBoard(board);
        changeRules(rules); 
        changeState(new PlacingState(tileCounts[Piece.Color.PLAYER_1], tileCounts[Piece.Color.PLAYER_2], startingColor));
    }

    public void changeBoard(SteppingStonesBoard board) { _board = board; }

    public void changeRules(Rules rules) { _rules = rules; }

    public void changeState(BoardState state) { _currentState = state; }

    public BoardState currentState() { return _currentState; }

    public void selectSquare(Location location) { _currentState = _currentState.processInput(_board, _rules, location); }

    public SteppingStonesBoard board() {  return _board; }

    public Rules rules() { return _rules; }
}