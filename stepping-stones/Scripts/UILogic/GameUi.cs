using Godot;
using System;

public partial class GameUi : Control
{
	// Called when the node enters the scene tree for the first time.
	
	[Signal]
	public delegate void ResetGameEventHandler();
	[Signal]
	public delegate void SaveGameEventHandler();
	[Signal]
	public delegate void LoadGameEventHandler();
	

	private void OnResetButtonPressed() 
	{
		EmitSignal(SignalName.ResetGame);
	}
	private void OnSaveButtonPressed() 
	{
		EmitSignal(SignalName.SaveGame);
	}

	private void OnLoadButtonPressed()
	{
		EmitSignal(SignalName.LoadGame);
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
