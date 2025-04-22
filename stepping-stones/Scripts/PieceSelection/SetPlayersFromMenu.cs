using Godot;
using System;

public partial class SetPlayersFromMenu : Node2D
{

	private void onCreateLocalGame() {
		PlayerManager.reset(); 
		PlayerManager.setPlayerMode(PlayerColor.PLAYER_1, PlayerType.LOCAL);
		PlayerManager.setPlayerMode(PlayerColor.PLAYER_2, PlayerType.LOCAL);
	}

	private void onLoadLocalGame() {
		PlayerManager.reset(); 
		PlayerManager.setPlayerMode(PlayerColor.PLAYER_1, PlayerType.LOCAL);
		PlayerManager.setPlayerMode(PlayerColor.PLAYER_2, PlayerType.LOCAL);
	}

	private void onMakeOnlineGame() {
		PlayerManager.reset(); 
		PlayerManager.setPlayerMode(PlayerColor.PLAYER_1, PlayerType.LOCAL);
		PlayerManager.setPlayerMode(PlayerColor.PLAYER_2, PlayerType.ONLINE);
	}

	private void onJoinOnlineGame() {
		PlayerManager.reset(); 
		PlayerManager.setPlayerMode(PlayerColor.PLAYER_2, PlayerType.LOCAL);
		PlayerManager.setPlayerMode(PlayerColor.PLAYER_1, PlayerType.ONLINE);
	}
}
