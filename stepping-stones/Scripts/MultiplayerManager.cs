using Godot;
using System;
using System.Collections.Generic;

public partial class MultiplayerManager : Node
{
    private WebRtcPeerConnection peerConnection;
    private Timer connectionCheckTimer;
    private Godot.Collections.Array iceServers = new Godot.Collections.Array
    {
        new Godot.Collections.Dictionary { { "urls", "stun:stun.l.google.com:19302" } }
    };


    public override void _Ready()
    {
        GD.Print("MultiplayerManager initialized.");
        InitializePeerConnection();
        StartConnectionMonitor();
    }

    private void InitializePeerConnection()
    {
        GD.Print("Initializing WebRTC Peer Connection...");
        peerConnection = new WebRtcPeerConnection();

        // WebRTC Configuration
        var config = new Godot.Collections.Dictionary
        {
            { "iceServers", iceServers }
        };


        if (peerConnection.Initialize(config) != Error.Ok)
        {
            GD.PrintErr("Failed to initialize WebRTC peer connection.");
            return;
        }

        // Connect WebRTC signals to C# methods
        peerConnection.SessionDescriptionCreated += OnSessionDescriptionCreated;
        peerConnection.IceCandidateCreated += OnIceCandidateCreated;

        GD.Print("WebRTC Peer Connection initialized successfully.");
    }

    private void StartConnectionMonitor()
    {
        connectionCheckTimer = new Timer();
        connectionCheckTimer.WaitTime = 1.0; // Check connection state every second
        connectionCheckTimer.OneShot = false;
        connectionCheckTimer.Timeout += OnConnectionStateCheck;
        AddChild(connectionCheckTimer);
        connectionCheckTimer.Start();
    }

    private void OnConnectionStateCheck()
    {
        WebRtcPeerConnection.ConnectionState state = peerConnection.GetConnectionState();
        GD.Print($"WebRTC Connection State: {state}");
    }

    private void OnSessionDescriptionCreated(string type, string sdp)
    {
        GD.Print($"Session Description Created: {type}");
        SendSignalingMessage(type, sdp);
    }

    private void OnIceCandidateCreated(string media, long index, string candidate)
    {
        GD.Print($"ICE Candidate Created: Media={media}, Index={index}, Candidate={candidate}");
        SendSignalingMessage("ice", candidate);
    }

    private void SendSignalingMessage(string messageType, string data)
    {
        GD.Print($"[SIGNALING] Sending {messageType} data.");
        // TODO: Implement actual signaling mechanism (WebSocket or manual exchange)
    }
}
