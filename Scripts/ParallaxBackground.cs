    using Godot;
using System;

public class ParallaxBackground : Godot.ParallaxBackground {
    [Export]
    public float ScrollingSpeed = 500;

    public override void _Ready() {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta) {
        ScrollOffset = new Vector2(ScrollOffset.x + (ScrollingSpeed * delta), ScrollOffset.y);
    }

    public Vector2 getScrollOffset(){
        return this.ScrollOffset;
    }
}
