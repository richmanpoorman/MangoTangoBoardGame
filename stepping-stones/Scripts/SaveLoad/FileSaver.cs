using System;
public interface FileSaver {
    public void SaveGame(Board board, PlayerColor turn, int p1Tiles, 
                            int p2Tiles, GamePhase phase, String path);
    public (SteppingStonesBoard, PlayerColor, int, int, 
        GamePhase phase) LoadGame(String fileName);
}