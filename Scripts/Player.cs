using Godot;
using System;

public class Player : BaseEntity {
    [Export]
    public int bulletPoolSize = 5;
    [Export]
    public float timeBetweenBullets = 0.2F;
    private Boolean scaling = false;
    
    private Vector2 screenSize;
    private float timeSinceLastBullet = 0.0F;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    private Bullet[] bullets;
    public override void _Ready() {
        this.screenSize = GetViewportRect().Size;

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

    private float Clamp(float v, float min, float max){
        if (v < min) return min;
        if (v > max) return max;
        return v;
    }

    private bool InRange(float v, float min, float max){
        return (v >= min) && (v <= max);
    }
    public override void _Process(float delta) {
        // Accelerate if keys pressed.
        if (Input.IsActionPressed("move_right"))    velocity.x += 20;
        if (Input.IsActionPressed("move_left"))     velocity.x -= 20;
        if (Input.IsActionPressed("move_up"))       velocity.y -= 20;
        if (Input.IsActionPressed("move_down"))     velocity.y += 20;

        // Check movement constraints
        Vector2 spriteSize = GetSpriteSize("AnimatedSprite");
        Vector2 minCoords = new Vector2(spriteSize.x/2, spriteSize.y/2);
        Vector2 maxCoords = new Vector2((this.screenSize.x - spriteSize.x/2), (this.screenSize.y - spriteSize.y/2));

        if (!InRange(this.Position.x, minCoords.x, maxCoords.x) || !InRange(this.Position.y, minCoords.y, maxCoords.y)){
            this.Position = new Vector2(
                            Clamp(this.Position.x, minCoords.x, maxCoords.x),
                            Clamp(this.Position.y, minCoords.y, maxCoords.y));
            velocity.x = 0;
            velocity.y = 0;
        }

        this.timeSinceLastBullet += delta;
        // Shoot
        if (this.timeSinceLastBullet > this.timeBetweenBullets) {
            if (Input.IsActionPressed("shoot_1") && Shoot(Bullet.BulletTypeEnum.GROWER) == true) this.timeSinceLastBullet = 0.0F;
            if (Input.IsActionPressed("shoot_2") && Shoot(Bullet.BulletTypeEnum.SHRINKER) == true) this.timeSinceLastBullet = 0.0F;
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

    public override void OnCollision(Godot.Object body) {
        GD.Print("Player collided with " + body.GetType());
        Bullet objectAsBullet = body as Bullet;
        if (objectAsBullet == null){
            GD.Print("-- Object was not a bullet. Self-destroying");
            // Collisions with anything (except bullets, ironically) = death
            GetParent().RemoveChild(this);
            QueueFree();
        }   
    }
}
