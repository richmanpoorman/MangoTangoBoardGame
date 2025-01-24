using System;
public interface FileSaver {
    public void SaveGame(Board board);
    public Board LoadGame(String fileName);
}