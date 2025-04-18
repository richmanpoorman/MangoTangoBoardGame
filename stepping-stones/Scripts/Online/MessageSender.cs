using Godot;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;
using Microsoft.VisualBasic;
using System.Linq;

public partial class MessageSender
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	public string ipAddr; 
	public int port; 
	
	public int messVersion;

	private int localPort;

	public struct ipPort {
		public ipPort(string ip, int port) {
			this.ip = ip; this.port = port;
		}
		public string ip;
		public int port;
	}
	#nullable enable
	Socket? client = null;
	
	public static string Pad4(int num) {
		string res = "";
        for (int i = 0; i < (4-num.ToString().Length); i++) {
            res += "0";
        }
        return res + num.ToString();
	}

	private async Task<ipPort> GetHostIpAsync(Socket client, string roomID) {
		byte[] message = Encoding.UTF8.GetBytes(Pad4(messVersion) + Pad4(2) + Pad4(roomID.Length) + roomID);
		await client.SendAsync(message);
		byte[] buf = new byte[1024];

		await client.ReceiveAsync(buf, SocketFlags.None);
		
		return BufToIp(buf, 9);
	}
	public static ipPort BufToIp(byte[] buf, int start) {
		
		ArraySegment<byte> ipbytes = new ArraySegment<byte>(buf, start, 4);
		ArraySegment<byte> portbytes = new ArraySegment<byte>(buf, start + 4, 4);
		byte[] finportbytes = portbytes.ToArray();
		if (BitConverter.IsLittleEndian) {
			finportbytes = portbytes.Reverse().ToArray();
		}
		int port = BitConverter.ToInt32(finportbytes);
		string ip = ipbytes[0] + "." + ipbytes[1] + "." + ipbytes[2] 
								   + "." + ipbytes[3];
		return new ipPort(ip, port);
	}
	public async Task<(Socket, ipPort)>  JoinRoomAsync(string roomID){
		IPEndPoint endPoint = new (IPAddress.Parse(ipAddr), port);
		using Socket client = new(
    		endPoint.AddressFamily, 
    		SocketType.Stream, 
    		ProtocolType.Tcp);
		GD.Print("client socket made");
		await client.ConnectAsync(endPoint);
		GD.Print("client connected");
		return (client, await GetHostIpAsync(client, roomID));
		

	}

	public async Task<string> DebugJoinRoom(string roomID, string prepend) {
		IPEndPoint endPoint = new (IPAddress.Parse(ipAddr), port);
		using Socket client = new(
    		endPoint.AddressFamily, 
    		SocketType.Stream, 
    		ProtocolType.Tcp);
		GD.Print("client socket made");
		await client.ConnectAsync(endPoint);
		GD.Print("client connected");
		var ext =  await GetHostIpAsync(client, roomID);
		await client.DisconnectAsync(true);
		return await sendHandshakeAsync(client,prepend, ext);
	}

	public async Task<(Socket sock, string? roomCode)> MkSocketandCodeAsync () {
		IPEndPoint endPoint = new (IPAddress.Parse(ipAddr), port);
		Socket client = new (endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
		GD.Print("host sock made");
		await client.ConnectAsync(endPoint);
		return (client, await MakeRoomAsync(client));
	}

	public async Task<ipPort?> GetClientIpAsync(Socket client) {
		byte[] buffer = new byte[1024];
		
		GD.Print("waiting for packet");
		GD.Print($"host port: {((IPEndPoint)client.LocalEndPoint).Port}");
		int received = await client.ReceiveAsync(buffer, SocketFlags.None);
		GD.Print("ip packet received");
		if (received < 8) {
			return null;
		}
		int version = Int32.Parse(Encoding.UTF8.GetString(buffer, 0, 4));
		int mType = Int32.Parse(Encoding.UTF8.GetString(buffer, 4, 4));
		
		if (mType != 4) {
			return null;
		}

		return BufToIp(buffer, 9);
	}


	public async Task<string> DebugRunRoomAsync(string prepend = ""){
		IPEndPoint endPoint = new (IPAddress.Parse(ipAddr), port);
		Socket client = new (endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
		await client.ConnectAsync(endPoint);
		string roomID = ""; 
		try {
			roomID = await MakeRoomAsync(client);
		} catch (System.ApplicationException e){
			GD.Print(e.Message);
			return e.Message;
		}
		
		byte[] buffer = new byte[1024];
		
		int received = await client.ReceiveAsync(buffer, SocketFlags.None);
		if (received < 8) {
			return "bad len";
		}
		int version = Int32.Parse(Encoding.UTF8.GetString(buffer, 0, 4));
		int mType = Int32.Parse(Encoding.UTF8.GetString(buffer, 4, 4));
		if (mType != 4) {
			return "bad mess code";
		}
		await client.DisconnectAsync(true);
		return await sendHandshakeAsync(client,prepend, BufToIp(buffer, 9));
	}
	public static string ipFromInt(int num) {
		string ip = "";
		ip += num % 256 + ".";
		ip += num  / 256 % 256 + ".";
		ip += num  / 256 / 256 % 256 + ".";
		ip += num  / 256 / 256 / 256 % 256 + ".";
		return ip;
	}
	public async Task<string?> MakeRoomAsync(Socket client) {
		byte[] message = Encoding.UTF8.GetBytes(Pad4(messVersion) + Pad4(0));
		await client.SendAsync(message);
		byte[] buffer = new byte[100];
		int received = await client.ReceiveAsync(buffer, SocketFlags.None);
		GD.Print("Got a response");
		if (received < 8) {
			return null;
		}
		int recVersion = Int32.Parse(Encoding.UTF8.GetString(buffer, 0, 4));
		int mType = Int32.Parse(Encoding.UTF8.GetString(buffer, 4, 4));
		GD.Print($"mType is {mType}");
		if (mType != 1) {
			return null;
		}
		
		int length = Int32.Parse(Encoding.UTF8.GetString(buffer, 8, 4));
		GD.Print($"length is {length}");
		return Encoding.UTF8.GetString(buffer, 12, length);
	}

	public async Task<string> sendHandshakeAsync (Socket client, string prepend, ipPort ext) {
		string flags = "-w-";
		byte[] buf   = new byte[40];
		IPEndPoint endPoint = new (IPAddress.Parse(ext.ip), ext.port);
		await client.ConnectAsync(endPoint);
		await client.SendAsync(Encoding.ASCII.GetBytes(flags));
		await Task.Delay(100);
		await client.SendAsync(Encoding.ASCII.GetBytes(flags));
		await client.ReceiveAsync(buf, SocketFlags.None);
		string recFlags = Encoding.ASCII.GetString(buf);
		flags = prepend + "rwx";
		await client.SendAsync(Encoding.ASCII.GetBytes(flags));
		await client.ReceiveAsync(buf, SocketFlags.None);
		return Encoding.ASCII.GetString(buf);
	}

	public async Task sendHandshakeAsync (Socket client, ipPort ext) {
		string flags = "-w-";
		byte[] buf   = new byte[40];
		IPEndPoint endPoint = new (IPAddress.Parse(ext.ip), ext.port);
		await client.ConnectAsync(endPoint);
		await client.SendAsync(Encoding.ASCII.GetBytes(flags));
		await Task.Delay(100);
		await client.SendAsync(Encoding.ASCII.GetBytes(flags));
		await client.ReceiveAsync(buf, SocketFlags.None);
		string recFlags = Encoding.ASCII.GetString(buf);
		flags = "rwx";
		await client.SendAsync(Encoding.ASCII.GetBytes(flags));
		await client.ReceiveAsync(buf, SocketFlags.None);
	}
	#nullable disable
}
