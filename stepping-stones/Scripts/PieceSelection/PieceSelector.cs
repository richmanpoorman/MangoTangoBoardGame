using Godot;
using System;

using System.Collections.Generic;
using Godot.Collections;
using System.Runtime.CompilerServices;

public partial class PieceSelector : Sprite2D
{
    [Signal]
    public delegate void pieceSelectedEventHandler(InputEventMouse mouseEvent, Sprite2D piece);
    private InputEventMouse mouseEvent = null;
    public override void _Ready()
	{
        setUpStorage();
	}

    private void setUpStorage() {
        PlayerStorage ps = GetNode<PlayerStorage>("res://Templates/Board/PlayerStorage");
        pieceSelected += ps.onPieceSelected;
        Connect(PieceSelector.SignalName.pieceSelected, new Callable(ps,"onPieceSelected"));
    }
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseButton mouseEvent) {
            GD.Print("in click on piece");
            EmitSignal(SignalName.pieceSelected, mouseEvent, this);
        }
    }
}