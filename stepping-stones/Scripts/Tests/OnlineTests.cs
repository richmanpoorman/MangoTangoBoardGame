using Godot;
using System;
using GdUnit4;
using System.Data;
using static GdUnit4.Assertions;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;



[TestSuite]
public class OnlineTests {
	private MessageSender host;
	private MessageSender client;
	private int version = 1;
    private string ipAddr = "127.0.0.1";
    private int port = 4567;
	[Before]
	public void mkSender() {
		host   = new MessageSender();
		client = new MessageSender();

        host.messVersion   = version;
		host.ipAddr = ipAddr;
		host.port = port;
		client.messVersion = version; 
		client.ipAddr = ipAddr;
		client.port = port;
	}

	[TestCase]
	public async Task setUpHost() {
        IPEndPoint endPoint = new (IPAddress.Parse(ipAddr), port);
		using Socket client = new(
    		endPoint.AddressFamily, 
    		SocketType.Stream, 
    		ProtocolType.Tcp);
        await client.ConnectAsync(endPoint);
        // GD.PrintErr("Connected");
        string roomID = await host.MakeRoomAsync(client);
        // GD.PrintErr($"got code");
        // GD.Print($"room id is: {roomID}");
		client.Shutdown(SocketShutdown.Both);
		client.Close();
	}
	[TestCase]	
	public async Task connectToHost() {
        IPEndPoint endPoint = new (IPAddress.Parse(ipAddr), port);
		using Socket hostSock = new(
    		endPoint.AddressFamily, 
    		SocketType.Stream, 
    		ProtocolType.Tcp);
        await hostSock.ConnectAsync(endPoint);
        // GD.PrintErr("Connected");
        string roomID = await host.MakeRoomAsync(hostSock);
		// GD.Print("room made");
		(_, var hostInfo) = await client.JoinRoomAsync(roomID);
		// GD.Print("got host info");
		// GD.PushWarning($"host ip is {hostInfo.ip}:{hostInfo.port}");
		// AssertThat(false).IsEqual(true);
        // GD.PrintErr($"got code");
	}
	#nullable enable
	[TestCase]
	public async Task handshakeTest() {
		GD.Print("starting hs test");
		(Socket hostSock, string? roomID) = await host.MkSocketandCodeAsync();
		GD.Print("room made");
		if (roomID == null) {
			AssertThat(true).IsEqual(false);
			return;
		}
		(Socket clientSock, MessageSender.ipPort cc) = await client.JoinRoomAsync(roomID);
		GD.Print("room joined");
		MessageSender.ipPort? hc = await host.GetClientIpAsync(hostSock);
		GD.Print("got client ip");
		await hostSock.DisconnectAsync(true);
		await clientSock.DisconnectAsync(true);
		if (hc == null) {
			AssertThat(true).IsEqual(false);
			return;
		}
		GD.Print("both disconnected, hc not null");
		var hTask = Task.Run(() => host.sendHandshakeAsync(hostSock, "imhost", (MessageSender.ipPort)hc));
		var cTask = Task.Run(() => client.sendHandshakeAsync(clientSock, "imclient", (MessageSender.ipPort)cc));
		string hString = await hTask;
		string cString = await cTask;
		AssertThat(hString).IsEqual("imclientrwx");
		AssertThat(cString).IsEqual("imhostrwx");

	}
	#nullable disable
	}
