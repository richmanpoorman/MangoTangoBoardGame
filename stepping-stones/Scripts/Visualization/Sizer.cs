using Godot;
using System;
using System.Numerics;

public partial class Sizer : Node2D
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	BoardManager manager;
	[Export]
	int iniScale = 4;
	[Export]
	Camera2D camera;

	[Export]
	BoardDisplay display;
	public override void _Ready()
	{
		EventBus.Bus.onBoardReset += resize;
		CallDeferred(MethodName.resize);
		// manager.Position = GetViewportRect().GetCenter();
	}

	private void resize() {
		(Godot.Vector2 pos, Vector2I size) = display.getSizingInfo();
		Rect2 rect = new Rect2 (pos, size);
		camera.Position = rect.GetCenter();
		GD.Print($"rect is {rect}");
		QueueRedraw();
	}

    public override void _Draw()
    {
		//max width 21
		//max len  22
    }


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
