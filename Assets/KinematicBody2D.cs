using Godot;
using System;

public class KinematicBody2D : Godot.KinematicBody2D
{
    [Export]
    public int Speed = 400;

   
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    // called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta) {
        var velocity = new Vector2(); // The player's movement vector.
        if (Input.IsActionPressed("ui_right")) {
            velocity.x += 1;
        }

        if (Input.IsActionPressed("ui_left")) {
            velocity.x -= 1;
        }

        if (Input.IsActionPressed("ui_down")) {
            velocity.y += 1;
        }

        if (Input.IsActionPressed("ui_up")) {
            velocity.y -= 1;
        }
    }
}
