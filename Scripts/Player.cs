using Godot;
using System;

public class Player : BaseEntity {
    [Export]
    public int bulletPoolSize = 5;
    public float timeBetweenBullets = 0.2F;
    private Boolean scaling = false;
    private Vector2 velocity = new Vector2(0.0F, 0.0F);
    private float timeSinceLastBullet = 0.0F;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    private Bullet[] bullets;
    public override void _Ready() {
        Vector2 screenSize = GetViewportRect().Size;

        Node bulletTemplate = GetNode<Bullet>("Bullet");
        // Create the bullets
        GD.Print("Creating bullets");
        this.bullets = new Bullet[bulletPoolSize];
        for (int b = 0; b < bulletPoolSize; b++){
            bullets[b] = (Bullet) bulletTemplate.Duplicate();
            GetParent().CallDeferred("add_child", bullets[b]);
        }
    }

    private PackedScene _LoadObjectScene(String objectSceneName){
        String scenePath = "res://scenes/" + objectSceneName + ".tscn";
        GD.Print("Loading scene " + scenePath);
        return (PackedScene) ResourceLoader.Load(scenePath);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta){

        // Natural deacceleration
        velocity.x *= 0.9F;
        velocity.y *= 0.9F;

        if (Math.Abs(velocity.x) < 0.01) velocity.x = 0;
        if (Math.Abs(velocity.y) < 0.01) velocity.y = 0;

        // Accelerate if keys pressed.
        if (Input.IsActionPressed("move_right"))    velocity.x += 20;
        if (Input.IsActionPressed("move_left"))     velocity.x -= 20;
        if (Input.IsActionPressed("move_up"))       velocity.y -= 20;
        if (Input.IsActionPressed("move_down"))     velocity.y += 20;
        
        // TO-DO: Limit velocity and range of motion
        float x = this.Position.x + velocity.x * delta;
        float y = this.Position.y + velocity.y * delta;
        SetPosition(x,y);

        this.timeSinceLastBullet += delta;
        // Shoot
        if (this.timeSinceLastBullet > this.timeBetweenBullets) {
            if (Input.IsActionPressed("shoot_1") && Shoot(Bullet.BulletTypeEnum.GROWER) == true) this.timeSinceLastBullet = 0.0F;
            if (Input.IsActionPressed("shoot_2") && Shoot(Bullet.BulletTypeEnum.SHRINKER) == true) this.timeSinceLastBullet = 0.0F;
        }

        // Test
        if (Input.IsActionPressed("grow")) {
            if (scaling == false) Grow();
            scaling = true;
        }else if (Input.IsActionPressed("shrink")) {
            if (scaling == false) Shrink();
            scaling = true;
        } else {
            scaling = false;
        }
    }

    private bool Shoot(Bullet.BulletTypeEnum bulletType){
        int baseSize = 64; //TO-DO: Make a constant or get from sprite size
        // Find a bullet that is inactive
        for (int b = 0; b < this.bullets.Length; b++) {
            Bullet bullet = this.bullets[b];
            if (bullet.IsAlive() == false){
                bullet.Spawn(bulletType, this.Position.x + baseSize/2*this.Scale.x, this.Position.y);
                GD.Print("pew (" + bulletType.ToString() + ")");
                return true;
            }
        }
        return false;
    }
}
