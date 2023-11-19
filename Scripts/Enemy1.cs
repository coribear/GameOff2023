using Godot;
using System;

public class Enemy1 : BaseEnemy {
    public override void _Ready() {
    }
    public override void DoMovement(float delta) {
        // Very basic motion. Keep our velocity constant, to the left, as long as we
        // are inside the screen
        this.velocity = new Vector2(-200, 0);
    }
}
