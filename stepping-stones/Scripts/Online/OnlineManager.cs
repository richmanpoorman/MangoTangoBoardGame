using Godot;
using System;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

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
	private int hostPort = 5000;
	[Export]
	private int clientPort = 5001;
	private ENetMultiplayerPeer peer = new ENetMultiplayerPeer();
	public override void _Ready()
	{
		_bus = EventBus.Bus;
		sender = new MessageSender();
		sender.ipAddr = ipAddr;
		sender.port = remotePort;
		sender.messVersion = version;
		_bus.onMakeRoom += makeRoom;
		_bus.onJoinRoom += joinRoom;
	}
	private async void makeRoom() {
		(Socket sock, string roomCode) = await sender.MkSocketandCodeAsync();
		_bus.EmitSignal(EventBus.SignalName.onRoomCodeReceived, roomCode);
		MessageSender.ipPort? client = await sender.GetClientIpAsync(sock);
		if (client == null) {
			throw new ApplicationException("received null client");
		}
		await sock.DisconnectAsync(true);
		string result = await sender.sendLocalHSAsync(hostPort, clientPort);
		if (!result.Contains("rwx")) {
			throw new ApplicationException("can't establish handshake");
		}
		
		peer.CreateServer(hostPort);
		Multiplayer.MultiplayerPeer = peer;

	}
	private async void joinRoom(string roomCode) {
		(Socket sock, MessageSender.ipPort ipp) = await sender.JoinRoomAsync(roomCode);
		string result = await sender.sendLocalHSAsync(clientPort, hostPort);
		peer.CreateClient(ipAddr, hostPort, 0, 0, 0, clientPort);
	}

	// don't manually call this unless you know what you're doing, only public for rpc functionality
	[Rpc]
	public void synchronizeRules (int sWeight, bool offPush, bool scoutDiv) {
		//TODO: make work with rule components
		
		SetRules.setRules(sWeight, offPush, scoutDiv);
	} 
	[Rpc]
	public void synchronizeBoard (string [,] boardRep, int p1Tiles, int p2Tiles, PlayerColor turn) {
		//TODO: make work with rule components
		
		
	} 

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
