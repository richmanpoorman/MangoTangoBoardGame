
using Godot;

public interface MoveSelector {
    public void setPlayer(PlayerColor playerColor);
    public PlayerColor player(); // Which player this selector selects moves for
    public Location selection(); 
    public void emitMove(); 
    
}