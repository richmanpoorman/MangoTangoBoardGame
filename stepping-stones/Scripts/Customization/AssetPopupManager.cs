using Godot;
using System;

public partial class AssetPopupManager : Node2D
{
	[Export]
	private Node2D assetCreator; 

	private void _onCustomizationButtonClick() {
		assetCreator.Visible = true; 
	}
}
