using System;
public interface FileSaver {
    public void SaveGame(Board board, String path);
    public SteppingStonesBoard LoadGame(String fileName);
}