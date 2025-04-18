using Godot;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;
using Microsoft.VisualBasic;
using System.Linq;

public partial class MessageSender : Node
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

	public class Shake {
		
		public bool wrote = false;
		public bool read = false;
		public bool both = false;

        public override string ToString() {
			string val = "";
			val += read? "r"  : "-";
			val += wrote?  "w" : "-"; 
			val += both?  "x"  : "-";
			return val;
        }
		public static Shake ofString(string s) {
			Shake val = new Shake();
			val.read = s.Contains("r");
			val.wrote = s.Contains("w");
			val.both = s.Contains("x");
			return val;
		}

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
		GD.Print($"port pre flip is: {BitConverter.ToInt32(finportbytes)}");
		if (BitConverter.IsLittleEndian) {
			finportbytes = portbytes.Reverse().ToArray();
		}
		int port = BitConverter.ToInt32(finportbytes);
		GD.Print($"port post flip is: {port}");
		string ip = ipbytes[0] + "." + ipbytes[1] + "." + ipbytes[2] 
								   + "." + ipbytes[3];
		return new ipPort(ip, port);
	}
	public async Task<(Socket, ipPort)>  JoinRoomAsync(string roomID){
		IPEndPoint endPoint = new (IPAddress.Parse(ipAddr), port);
		Socket client = new(
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

		return BufToIp(buffer, 8);
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
		GD.Print($"port is {ext.port}");
		string flags = "-w-";
		byte[] buf   = new byte[40];
		IPEndPoint endPoint = new (IPAddress.Parse(ext.ip), ext.port);
		try {
			await client.ConnectAsync(endPoint);
			await client.SendAsync(Encoding.ASCII.GetBytes(flags));
		} catch (Exception e) {
			GD.Print(e.Message);
			await Task.Delay(100);
			await client.ConnectAsync(endPoint);
			await client.SendAsync(Encoding.ASCII.GetBytes(flags));
		}
		
		await client.ReceiveAsync(buf, SocketFlags.None);
		string recFlags = Encoding.ASCII.GetString(buf);
		flags = prepend + "rwx";
		await client.SendAsync(Encoding.ASCII.GetBytes(flags));
		await client.ReceiveAsync(buf, SocketFlags.None);
		return Encoding.ASCII.GetString(buf);
	}

	public async Task sendHandshakeAsync (Socket client, ipPort ext) {
		// GD.Print($"port is {ext}");
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
	public async Task<string> sendClientHSAsync (int localPort, ipPort ext, string prepend = "", float time = 10f, float wait = 0.2f) {
		Shake ourShake = new Shake();
		ourShake.wrote = true; 
		PacketPeerUdp udp = new PacketPeerUdp();
		udp.Bind(localPort);
		udp.SetDestAddress(ext.ip, ext.port);
		byte[] buf   = new byte[40];
		// IPEndPoint endPoint = new (IPAddress.Parse(ext.ip), ext.port);
		// udp.Connect(endPoint);
		try {
			udp.PutPacket(Encoding.ASCII.GetBytes(ourShake.ToString()));
		} catch (Exception e) {
			GD.Print(e.Message);
			await Task.Delay(100);
			udp.PutPacket(Encoding.ASCII.GetBytes(ourShake.ToString()));
		}
		string prefix = "";
		while (time >= 0) {
			GD.Print($"enterd loop");
			while (udp.GetAvailablePacketCount() > 0) {
				GD.Print($"curr packet count is: {udp.GetAvailablePacketCount()}");
				var p = udp.GetPacket();
				string recf = Encoding.ASCII.GetString(p);
				string[] parts = recf.Split(":");
				prefix = parts[0];
				Shake incShake = Shake.ofString(parts[1]);
				if (incShake.wrote) {
					ourShake.read = true;
				}
				if (incShake.read) {
					ourShake.both = true;
				}
				if (ourShake.both && incShake.both) {
					time = 0;
				}
				
			}
			await Task.Delay((int)(wait * 1000f));
			udp.PutPacket(Encoding.ASCII.GetBytes(prepend + ":" + ourShake.ToString()));
			time -= wait;
		}
		return prefix;
	}

	public async Task sendHostHSAsync (ENetConnection host, ipPort ext, string prepend = "", float time = 10f, float wait = 0.2f) {
		string flags = prepend + ":" + "rwx";
		// var peer = new ENetMultiplayerPeer();
		// peer.CreateServer(port);
		// Multiplayer.MultiplayerPeer = peer;
		// IPEndPoint endPoint = new (IPAddress.Parse(ext.ip), ext.port);
		while (time >= 0) {
			host.SocketSend(ext.ip, ext.port, Encoding.ASCII.GetBytes(flags));
			await Task.Delay((int)(wait * 1000f));
			time -= wait;
		}
		
		
		
	}


	#nullable disable
}

