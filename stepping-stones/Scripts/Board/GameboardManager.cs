
public interface GameboardManager { 

    public Board board(); 
    public PlayerColor playerTurn(); 
    public void setTurn(PlayerColor turn); 
    public void setRules(Rules rules); 
    public void setBoard(SteppingStonesBoard board);
    public GamePhase phase(); 
    public void setPhase(GamePhase phase); 
    public int playerTileCount(PlayerColor player);
    public void setTileCount(PlayerColor player, int count); 

}