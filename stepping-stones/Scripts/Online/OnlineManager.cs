using Godot;
using System;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using Godot.NativeInterop;

public partial class OnlineManager : Node
{
	// Called when the node enters the scene tree for the first time.
	private EventBus _bus;
	private MessageSender sender;
	[Export]
	private int version = 1;
    private string ipAddr = "127.0.0.1";
    private int remotePort = 4567;
	[Export]
	private int hostPort = 6666;
	[Export]
	private int clientPort = 5696;

	[Export]
	private bool LAN = true; 
	private ENetMultiplayerPeer peer;
	public override void _Ready() {
		_bus = EventBus.Bus;
		sender = new MessageSender();
		sender.ipAddr = ipAddr;
		sender.port = remotePort;
		sender.messVersion = version;
		_bus.onMakeRoom += makeRoom;
		_bus.onJoinRoom += joinRoom;
		Multiplayer.PeerConnected += wrapBoardSync;
		peer = new ENetMultiplayerPeer();
	}
	private void wrapBoardSync(long id) {
		if (Multiplayer.IsServer()) {
			createBoardSync();
		}
	}
	private async void makeRoom() {
		(Socket sock, string roomCode) = await sender.MkSocketandCodeAsync();
		_bus.EmitSignal(EventBus.SignalName.onRoomCodeReceived, roomCode);
		MessageSender.ipPort? client = await sender.GetClientIpAsync(sock);
		if (client == null) {
			throw new ApplicationException("received null client");
		}
		sock.Close();
		string result = "";

		if (LAN) {
			result = await sender.sendLocalHSAsync(hostPort, clientPort);
		} else {
			if (client != null) {
				result = await sender.sendOnlineHSAsync(hostPort, (MessageSender.ipPort)client);
			} else {
				throw new ApplicationException("null client");
			}
		}
		if (!result.Contains("rwx")) {
			throw new ApplicationException("can't establish handshake");
		}
		GD.Print($"{hostPort}");
		
		peer.CreateServer(hostPort);
		Multiplayer.MultiplayerPeer = peer;
	}
	private async void joinRoom(string roomCode) {
		(Socket sock, MessageSender.ipPort ipp) = await sender.JoinRoomAsync(roomCode);
		sock.Close();
		string result = await sender.sendLocalHSAsync(clientPort, hostPort);
		await Task.Delay(10);
		peer.CreateClient(ipAddr, hostPort, 0, 0, 0, clientPort);
	}

	// don't manually call this unless you know what you're doing, only public for rpc functionality
	[Rpc]
	private void synchronizeRules (int sWeight, bool offPush, bool scoutDiv) {
		//TODO: make work with rule components
		
		SetRules.setRules(sWeight, offPush, scoutDiv);
	} 


	#nullable enable
	private void createBoardSync () {
		SceneManager sm = SceneManager.Instance;
		Board board = sm.board;
		int[] size = board.size();
		int numRows = size[0];
		int numCols = size[1];
		Godot.Collections.Array<string> arr = new Godot.Collections.Array<string>();
		
		for(int i = 0; i < numRows; i++) {
			for (int j = 0; j < numCols; j++) {
				Location curLoc = Location.at(i, j);
				Tile? currTile = board.tileAt(curLoc);
				if (currTile is null) {
					continue;
				}
				char color = currTile.color() == PlayerColor.PLAYER_1 ? 'r' : 'b'; 
				string rep = i + ":" + j + ":" + color + "t";
				if (board.scoutAt(curLoc) is not null) {
					rep += "s";
				}
				arr.Add(rep);
			}
		}
		Rpc(MethodName.synchronizeBoard, arr, numRows, numCols, sm.p1Tiles, sm.p2Tiles, (int)sm.turn, sm.newGame);
		Rpc(MethodName.synchronizeRules, SetRules.scoutWeight, SetRules.hasOffensivePush, SetRules.hasScoutRequiredToDivide);
	}
	
	#nullable disable
	
	[Rpc(MultiplayerApi.RpcMode.Authority, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void synchronizeBoard (Godot.Collections.Array<string> arr, int numRows, int numCols, int p1Tiles, int p2Tiles, PlayerColor turn, bool newGame) {
		//TODO: make work with rule components
		SceneManager.Instance.p1Tiles = p1Tiles;
		SceneManager.Instance.p2Tiles = p2Tiles;
		SceneManager.Instance.turn    = turn;
		SceneManager.Instance.newGame = newGame;
		SteppingStonesBoard board = new GridSteppingStonesBoard(numRows, numCols);
		foreach (string square in arr) {
			string[] elts = square.Split(":");
			int i = int.Parse(elts[0]);
			int j = int.Parse(elts[1]);
			string cur = elts[2];
			if (cur.Contains("t")) {
				PlayerColor color = cur.Contains("r") ? PlayerColor.PLAYER_1 : PlayerColor.PLAYER_2;
				Tile t = new Tile(color);
				board.addTile(t, Location.at(i, j));
				if (cur.Contains("s")) {
					board.addScout(new Scout(color), Location.at(i, j));
				}
			}
		}
		SceneManager.Instance.board = board;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
