using Godot;
using System;

public class Bullet : Area2D {
    [Export]
    float speed = 20.0F;

    [Signal]
    public delegate void Hit();

    private bool alive = false;

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        SetAlive(false);
        GD.Print("Bullet spawn");
    }

    public void Spawn(float x, float y){
        this.Position = new Vector2(x, y);
        SetAlive(true);
    }

    private void SetAlive(bool flag){
        this.alive = flag;
        this.Visible = this.alive;
    }

    public bool IsAlive(){
        return this.alive;
    }

    public override void _Process(float delta){
        if (this.IsAlive()){
            this.Position = new Vector2(this.Position.x + this.speed, this.Position.y);
            // Check if outside the screen
            if (this.Position.x > GetViewportRect().Size.x) {
                GD.Print("bullet out of sight");
                SetAlive(false);
            }
        }
    }
}
