using Godot;
using System;

public class StaticObject : StaticBody2D {
    private Growable growableScale;
    [Export]
    public float growSize;
    [Export]
    public float smallSize;
    
    // For some reason _Ready works with this static object even when it doesn't with our BaseEntity
    public override void _Ready() {
        this.growableScale = new Growable(smallSize, this.Scale.x, growSize);
    }

    public Growable GetGrowableScale(){
        return this.growableScale;
    }

    private void SetScale(float s){
        this.Scale = new Vector2(s, s);
    }

    public override void _Process(float delta){
        // Nothing to do if we still don't have an instance of our growth tracker
        if (this.growableScale == null) return;

        // Keep track of our growable object reference
        float scaleDiff = this.growableScale.GetScale() - this.Scale.x;
        SetScale(this.Scale.x + scaleDiff*0.1F);
    }
}
