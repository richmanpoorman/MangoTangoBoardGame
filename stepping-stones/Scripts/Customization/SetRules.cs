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
    public static int scoutWeight {private set; get;} = 1;
    public static bool hasOffensivePush {private set; get;} = false;
    public static bool hasScoutRequiredToDivide {private set; get;} = false; 


    public static void setRules(int _scoutWeight, bool _hasOffensivePush, bool _hasScoutRequiredToDivide) {
        scoutWeight = _scoutWeight;
        hasOffensivePush = _hasOffensivePush;
        hasScoutRequiredToDivide = _hasScoutRequiredToDivide;  
        rules = new ComposableRules(scoutWeight, hasScoutRequiredToDivide, hasOffensivePush);
    }

    private void onSetRules() {
        setRules((int)scoutWeightOption.Value, offensivePushOption.ButtonPressed, scoutDividerOption.ButtonPressed);
        root.Visible = false; // Close after selection
    }
}
