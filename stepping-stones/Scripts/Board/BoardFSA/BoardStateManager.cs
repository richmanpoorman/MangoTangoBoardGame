

using System.Collections.Generic;

public class BoardStateManager : BoardStateMachine
{
    public enum GamePhase {
		PLACE, MOVE 
	}

    private BoardState _currentState; 
    private SteppingStonesBoard _board; 
    private Rules _rules;
    
    private Dictionary<PlayerColor, int> startingCounts;
    private PlayerColor startColor;
    public BoardStateManager(SteppingStonesBoard board, Rules rules, Dictionary<PlayerColor, int> tileCounts, PlayerColor startingColor) {
        changeBoard(board);
        changeRules(rules); 
        changeState(new PlacingState(tileCounts, startingColor));
        startingCounts = tileCounts;
        startColor = startingColor;
    }

    public void changeBoard(SteppingStonesBoard board) { _board = board; }

    public void changeRules(Rules rules) { _rules = rules; }

    public void changeState(BoardState state) { _currentState = state; }
    public void changeState(GamePhase phase) {
        switch(phase) {
            case GamePhase.PLACE:
                changeState(new PlacingState(startingCounts, startColor));
            break; 
            case GamePhase.MOVE:
                changeState(new MovementState(startColor));
            break; 
        }
    }

    public BoardState currentState() { return _currentState; }

    public void selectSquare(Location location) { _currentState = _currentState.processInput(_board, _rules, location); }

    public SteppingStonesBoard board() {  return _board; }

    public Rules rules() { return _rules; }

    public int tileCount(PlayerColor player) {
        if (_currentState is PlacingState placingState) 
            return placingState.tileCount(player);
        else return -1;
    }

    public void setTileCount(PlayerColor player, int count) {
        if (_currentState is PlacingState placingState) 
            placingState.setTileCount(player, count);
    }

    public void changeTurn(PlayerColor turn) { _currentState.changeTurn(turn); }

    public PlayerColor turn() { return _currentState.playerTurn(); }
}