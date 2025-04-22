using Godot;
using System;

public partial class RoomCodePopup : Node2D
{
	[Export]
	private Node2D root;

	[Export]
	private RichTextLabel roomCodeText; 

	private EventBus bus; 
    public override void _Ready()
    {
        bus = EventBus.Bus; 
		bus.onRoomCodeReceived += onRoomCodeReceived; 
    }

    public override void _ExitTree()
    {
        bus.onRoomCodeReceived -= onRoomCodeReceived; 
    }

	private void onRoomCodeSubmit() {
		root.Visible = false; 
	}

	private void onRoomCodeReceived(string roomCode) {
		roomCodeText.Text = $"[center]Room Code: {roomCode}[/center]";
	}
}
