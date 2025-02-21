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
	
	[Export]
	public RichTextLabel turnLabel;

	private Piece.Color currentPlayer;

	private BoardManager manager;

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
		manager = GetParent().GetNode<BoardManager>("Main/BoardManager");
		currentPlayer = manager.playerTurn();
		switchColor();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Piece.Color tempTurn = manager.playerTurn();
		if (currentPlayer != tempTurn ){
			currentPlayer = tempTurn;
			switchColor();
		}
	}
	private void switchColor () {
		switch(currentPlayer) {
			case Piece.Color.PLAYER_1:
				turnLabel.Text = "Current Turn is: [color=firebrick] Player 1 [/color]";
				break;
			case Piece.Color.PLAYER_2:
				turnLabel.Text = "Current Turn is: [color=dodger_blue] Player 2 [/color]";
				break;
			default:
					break;
			}
	}
	///Some code for how reset maybe works
	///initialBoard = new setup
}
