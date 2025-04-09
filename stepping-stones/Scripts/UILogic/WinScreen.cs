using Godot;
using System;

public partial class WinScreen : Control
{
	[Export]
	private Control uiMenu;
	[Export]
	private Control buttonAlign;	
	[Export]
	private Control winText;		
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnPlayButtonPressed()
	{
		uiMenu.Visible = true;
		buttonAlign.Visible = false;
		winText.Visible = false;
	}

	private void OnGoMainButtonPressed()
	{
		SceneManager.Instance.goToTitleScreen();
	}
}
