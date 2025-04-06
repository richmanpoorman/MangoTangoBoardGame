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
	#nullable enable
	Socket? client = null;
	public static string Pad4(int num) {
		string res = "";
        for (int i = 0; i < (4-num.ToString().Length); i++) {
            res += "0";
        }
        return res + num.ToString();
	}

	private async Task<(string hostIp, int port)> GetHostIpAsync(Socket client, string roomID) {
		byte[] message = Encoding.UTF8.GetBytes(Pad4(messVersion) + Pad4(2) + Pad4(roomID.Length) + roomID);
		await client.SendAsync(message);
		byte[] buf = new byte[1024];

		await client.ReceiveAsync(buf, SocketFlags.None);
		
		return BufToIp(buf, 9);
	}
	public static (string hostIp, int port) BufToIp(byte[] buf, int start) {
		
		ArraySegment<byte> ipbytes = new ArraySegment<byte>(buf, start, 4);
		ArraySegment<byte> portbytes = new ArraySegment<byte>(buf, start + 4, 4);
		byte[] finportbytes = portbytes.ToArray();
		if (BitConverter.IsLittleEndian) {
			finportbytes = portbytes.Reverse().ToArray();
		}
		int port = BitConverter.ToInt32(finportbytes);
		string ip = ipbytes[0] + "." + ipbytes[1] + "." + ipbytes[2] 
								   + "." + ipbytes[3];
		return (ip, port);
	}
	public async Task<(string hostIp, int port)>  JoinRoomAsync(string roomID){
		IPEndPoint endPoint = new (IPAddress.Parse(ipAddr), port);
		using Socket client = new(
    		endPoint.AddressFamily, 
    		SocketType.Stream, 
    		ProtocolType.Tcp);
		GD.Print("client socket made");
		await client.ConnectAsync(endPoint);
		GD.Print("client connected");
		return await GetHostIpAsync(client, roomID);

	}

	public async Task RunRoomAsync(){
		IPEndPoint endPoint = new (IPAddress.Parse(ipAddr), port);
		using Socket client = new(
    		endPoint.AddressFamily, 
    		SocketType.Stream, 
    		ProtocolType.Tcp);
		await client.ConnectAsync(endPoint);
		string roomID = ""; 
		try {
			roomID = await MakeRoomAsync(client);
		} catch (System.ApplicationException e){
			GD.Print(e.Message);
			return;
		}
		
		byte[] buffer = new byte[1024];
		
		int received = await client.ReceiveAsync(buffer, SocketFlags.None);
		if (received < 8) {
			return;
		}
		int version = Int32.Parse(Encoding.UTF8.GetString(buffer, 0, 4));
		int mType = Int32.Parse(Encoding.UTF8.GetString(buffer, 4, 4));
		if (mType != 4) {
			return;
		}
		ArraySegment<byte> ccipbytes = new ArraySegment<byte>(buffer, 8, 4);
		int ccport = BitConverter.ToInt32(buffer, 12);
		string ccip = ccipbytes[0] + "." + ccipbytes[1] + "." + ccipbytes[2] 
								   + "." + ccipbytes[3];
	}
	public static string ipFromInt(int num) {
		string ip = "";
		ip += num % 256 + ".";
		ip += num  / 256 % 256 + ".";
		ip += num  / 256 / 256 % 256 + ".";
		ip += num  / 256 / 256 / 256 % 256 + ".";
		return ip;
	}
	public async Task<string> MakeRoomAsync(Socket client) {
		byte[] message = Encoding.UTF8.GetBytes(Pad4(messVersion) + Pad4(0));
		await client.SendAsync(message);
		byte[] buffer = new byte[100];
		int received = await client.ReceiveAsync(buffer, SocketFlags.None);
		GD.Print("Got a response");
		if (received < 8) {
			throw new ApplicationException($"expected at least 8 bytes, only got {received}");
		}
		int recVersion = Int32.Parse(Encoding.UTF8.GetString(buffer, 0, 4));
		int mType = Int32.Parse(Encoding.UTF8.GetString(buffer, 4, 4));
		GD.Print($"mType is {mType}");
		if (mType != 1) {
			throw new ApplicationException($"expected message type 1, got {mType}");
		}
		
		int length = Int32.Parse(Encoding.UTF8.GetString(buffer, 8, 4));
		GD.Print($"length is {length}");
		return Encoding.UTF8.GetString(buffer, 12, length);
	}
	#nullable disable
}
