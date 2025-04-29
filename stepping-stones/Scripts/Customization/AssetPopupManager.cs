using Godot;
using System;

public partial class AssetPopupManager : Node2D
{
	[Export]
	private Node2D assetCreator, rulesCreator; 

	private void _onCustomizationButtonClick() {
		assetCreator.Visible = !assetCreator.Visible; 
		rulesCreator.Visible = false;
	}
}
