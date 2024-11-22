using Godot;
using System;

public partial class Titlescreen : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	public PackedScene GameScene {get; set;}
	public override void _Ready()
	{
	}
	private void OnStartButtonPressed() {
		GetTree().ChangeSceneToPacked(GameScene);
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
