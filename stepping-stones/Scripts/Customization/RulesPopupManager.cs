using Godot;
using System;

public partial class RulesPopupManager : Node2D
{
	[Export]
	private Node2D rulesPopup, assetPopup; 

	private void onPressed() {
		rulesPopup.Visible = !rulesPopup.Visible; 
		assetPopup.Visible = false; 
	}
}
