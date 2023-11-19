using Godot;
using System;

public class Enemy2 : BaseEnemy {
    float tTime = 0;
    public override void DoMovement(float delta) {
        tTime += delta;
        // Very basic motion. Keep our velocity constant, to the left, as long as we
        // are inside the screen
        this.velocity = new Vector2(-200, 150*((float)Math.Sin(tTime*5)));
    }
}
