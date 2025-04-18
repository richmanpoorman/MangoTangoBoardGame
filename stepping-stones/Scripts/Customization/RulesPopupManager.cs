using Godot;
using System;

public partial class RulesPopupManager : Node2D
{
	[Export]
	private Node2D rulesPopup; 

	private void onPressed() {
		rulesPopup.Visible = true; 
	}
}
