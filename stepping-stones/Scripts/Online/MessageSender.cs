using Godot;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;

public partial class MessageSender : Node
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	string ipAddr; int port; int version;
	#nullable enable
	IPEndPoint? endPoint = null;
	Socket? client = null;
	public static string Pad4(int num) {
		string res = "";
        for (int i = 0; i < (4-num.ToString().Length); i++) {
            res += "0";
        }
        return res + num.ToString();
	}
	public async Task RunRoom(){
		if (endPoint == null) {
			endPoint = new (IPAddress.Parse(ipAddr), port);
		}
		using Socket client = new(
    		endPoint.AddressFamily, 
    		SocketType.Stream, 
    		ProtocolType.Tcp);
		await client.ConnectAsync(endPoint);
		string roomID = ""; 
		try {
			roomID = await MakeRoom(client);
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
	public async Task<string> MakeRoom(Socket client) {
		byte[] message = Encoding.UTF8.GetBytes(Pad4(version) + Pad4(1));
		await client.SendAsync(message);
		byte[] buffer = new byte[100];
		int received = await client.ReceiveAsync(buffer, SocketFlags.None);
		if (received < 8) {
			throw new ApplicationException($"expected at least 8 bytes, only got {received}");
		}
		int version = Int32.Parse(Encoding.UTF8.GetString(buffer, 0, 4));
		int mType = Int32.Parse(Encoding.UTF8.GetString(buffer, 4, 4));
		if (mType != 1) {
			throw new ApplicationException($"expected message type 1, got {mType}");
		}
		int length = Int32.Parse(Encoding.UTF8.GetString(buffer, 8, 4));
		return Encoding.UTF8.GetString(buffer, 8, length);
	}

	public void JoinRoom(string roomID) {

	}
	#nullable disable
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
