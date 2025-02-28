using Godot;
using System;

public partial class MainGame : Node2D
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	public BoardManager manager;
	private FileSaver saver =  new GameSaver();
	private SceneManager sceneManager;

	private EventBus _eventBus; 

	public override void _Ready()
	{
		_eventBus = EventBus.Bus;
		sceneManager = SceneManager.Instance;
		manager = GetNode<BoardManager>("Main/BoardManager");
		CallDeferred(MethodName.DeferredSetupCleanup);

		// Connect(EventBus.SignalName.onPlayerWin, Callable.From(handleWin));
		_eventBus.onPlayerWin += handleWin;
	}
	private void handleWin()
	{
		GetNode<Control>("GameUI").Visible = false;
		GetNode<Node2D>("Main").Visible = false;
		GetNode<Control>("WinScreen").Visible = true;
	}

	public void DeferredSetupCleanup() {
		GD.Print("I was deffered :)");
		manager.setBoard(sceneManager.board);
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnUISaveGame(String path) {
		saver.SaveGame(manager.board(), path);
	}
	public void OnUIResetGame() {
		manager.setBoard(new GridSteppingStonesBoard(manager.board().size()[0], manager.board().size()[1]));
		manager.onRestart();
	}
	public void OnUILoadGame(String path) {
		GD.Print("Game Loaded");
		manager.setBoard(saver.LoadGame(path));
		Board board = manager.board();
		int[] size = manager.board().size();
	}
}
