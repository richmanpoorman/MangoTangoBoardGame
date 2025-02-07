using Godot;
using System;

public partial class GameUi : Control
{
	// Called when the node enters the scene tree for the first time.
	
	[Signal]
	public delegate void ResetGameEventHandler();
	[Signal]
	public delegate void SaveGameEventHandler(String path);
	[Signal]
	public delegate void LoadGameEventHandler(String path);

	[Export]
	public FileDialog saveBox;
	
	[Export]
	public FileDialog loadBox;
	

	private void OnResetButtonPressed() 
	{
		EmitSignal(SignalName.ResetGame);
	}
	private void OnSaveButtonPressed() 
	{
		saveBox.Popup();
	}

	private void OnLoadButtonPressed()
	{
		loadBox.Popup();
	}
	private void OnSaveDialogFileSelected(String path) 
	{
		EmitSignal(SignalName.SaveGame, path);
	}
	private void OnLoadDialogFileSelected(String path) 
	{
		EmitSignal(SignalName.LoadGame, path);
	}
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	///Some code for how reset maybe works
	///initialBoard = new setup
}
