
public interface GameboardManager { 

    public Board board(); 
    public Piece.Color playerTurn(); 
    public void setTurn(Piece.Color turn); 
    public void setRules(Rules rules); 
    public void setBoard(SteppingStonesBoard board);
    public BoardManager.GamePhase phase(); 
    public void setPhase(BoardManager.GamePhase phase); 
    public int playerTileCount(Piece.Color player);
    public void setTileCount(Piece.Color player, int count); 

}