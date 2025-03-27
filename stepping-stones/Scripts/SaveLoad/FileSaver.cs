using System;
public interface FileSaver {
    public void SaveGame(Board board, Piece.Color turn, int p1Tiles, 
                            int p2Tiles, BoardManager.GamePhase phase, String path);
    public (SteppingStonesBoard, Piece.Color, int, int, 
        BoardManager.GamePhase phase) LoadGame(String fileName);
}