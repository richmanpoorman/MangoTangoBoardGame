using System;
public interface FileSaver {
    public void SaveGame(Board board);
    public SteppingStonesBoard LoadGame(String fileName);
}