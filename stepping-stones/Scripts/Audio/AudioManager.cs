using Godot;
using System;

public partial class AudioManager : Node2D
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	private AudioStreamPlayer2D audioPlayer; 
	[Export]
	private AudioStream tilePlaceSound, scoutMoveSound, tileMoveSound, tilePushSound; 

	private EventBus _eventBus;
	public override void _Ready() {
		_eventBus = EventBus.Bus; 
		_eventBus.onTilePlace += onTilePlace; 
		_eventBus.onScoutMove += onScoutMove; 
		_eventBus.onTileMove += onTileMove; 
		_eventBus.onTilePush += onTilePush; 	
	}

	private void playSound(AudioStream audio) {
		audioPlayer.Stream = audio; 
		audioPlayer.Play(); 
	} 
	
	public void onTilePlace() { playSound(tilePlaceSound); }

	public void onScoutMove() { playSound(scoutMoveSound); }

	public void onTileMove() { playSound(tileMoveSound); }

	public void onTilePush() { playSound(tilePushSound); }


}
