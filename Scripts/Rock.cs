using Godot;
using System;

public class Rock : StaticObject {
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta) {
        // Check if we are in the "grow" state
        if (this.GetGrowableScale() != null) {
            this.applyGravity = (this.GetGrowableScale().GetCurrentScaleSetting() == Growable.ScaleSetting.BIG);
        }
        
        // Call the base implementation
        base._Process(delta);
    }
}
