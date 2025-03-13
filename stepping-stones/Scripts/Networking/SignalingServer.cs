using Godot;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public partial class SignalingServer : Node
{
    private ClientWebSocket websocket;
    private Uri serverUri;
    public event Action<string, string> MessageReceived; // Event for receiving messages

    public SignalingServer(string serverUrl)
    {
        serverUri = new Uri(serverUrl);
        websocket = new ClientWebSocket();
    }

    public async void ConnectToServer()
    {
        try
        {
            GD.Print($"[SIGNALING] Connecting to {serverUri}...");
            await websocket.ConnectAsync(serverUri, CancellationToken.None);
            GD.Print("[SIGNALING] Connected to WebSocket server.");
            ReceiveMessages(); // Start listening for messages
        }
        catch (Exception e)
        {
            GD.PrintErr($"[SIGNALING] Connection failed: {e.Message}");
        }
    }

    private async void ReceiveMessages()
    {
        byte[] buffer = new byte[1024];
        while (websocket.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result = await websocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            GD.Print($"[SIGNALING] Received: {message}");

            // Extract message type (expected format: "type:data")
            string[] parts = message.Split(':', 2);
            if (parts.Length == 2)
            {
                MessageReceived?.Invoke(parts[0], parts[1]); // Trigger event
            }
        }
    }

    public async void SendMessage(string messageType, string data)
    {
        if (websocket.State != WebSocketState.Open)
        {
            GD.PrintErr("[SIGNALING] WebSocket is not connected!");
            return;
        }

        string message = $"{messageType}:{data}";
        byte[] bytes = Encoding.UTF8.GetBytes(message);
        await websocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        GD.Print($"[SIGNALING] Sent: {message}");
    }
}
