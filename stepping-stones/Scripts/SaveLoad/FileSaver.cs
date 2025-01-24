using System;
using Godot;

public interface FileSaver {
    public void SaveGame(Board board);
    public void LoadGame(Board board, String fileName);
    //load


    
}