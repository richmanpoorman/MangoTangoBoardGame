
using Godot;

public interface MoveSelector {
    public void setPlayer(Piece.Color playerColor);
    public Piece.Color player(); // Which player this selector selects moves for
    public Location selection(); 
    public void emitMove(); 
    
}