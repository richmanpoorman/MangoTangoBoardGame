using Godot;
using System;

public partial class SetRules : Node2D
{
    public static Rules rules { private set; get; } = new ComposableRules(1, false, false);

    [Export]
    private CheckBox offensivePushOption, scoutDividerOption; 
    
    [Export]
    private SpinBox scoutWeightOption; 

    [Export]
    private Node2D root; 

    private void setRules(int scoutWeight, bool hasOffensivePush, bool hasScoutRequiredToDivide) {
        rules = new ComposableRules(scoutWeight, hasScoutRequiredToDivide, hasOffensivePush);
    }

    private void onSetRules() {
        setRules((int)scoutWeightOption.Value, offensivePushOption.ButtonPressed, scoutDividerOption.ButtonPressed);
        root.Visible = false; // Close after selection
    }
}
