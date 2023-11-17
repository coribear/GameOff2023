using Godot;
using System;

public class Enemy1 : BaseEntity {
    
    public override void _Ready() {
    }

    public override void _Process(float delta) {
        // Very basic motion. Keep our velocity constant, to the left.
        this.velocity = new Vector2(-200, 0);
    }

    public override void OnCollision(Godot.Object body) {
        GD.Print("Enemy collided with " + body.GetType());
        StaticBody2D objectAsStaticBody = body as StaticBody2D;
        if (objectAsStaticBody != null){
            GD.Print("-- Object was a static body. Self-destroying");
            // Collisions with anything (except bullets, ironically) = death
            GetParent().RemoveChild(this);
            QueueFree();
        }
    }
}
