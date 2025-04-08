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
        GD.Print($"room id is: {roomID}");
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
		GD.Print("room made");
		var hostInfo = await client.JoinRoomAsync(roomID);
		GD.Print("got host info");
		GD.PushWarning($"host ip is {hostInfo.hostIp}:{hostInfo.port}");
		// AssertThat(false).IsEqual(true);
        // GD.PrintErr($"got code");
	}
	}
