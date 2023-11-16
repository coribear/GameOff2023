using Godot;
using System;

public class BaseEntity : KinematicBody2D {
    const float NORMAL_SIZE = 1.0F;
    private float currentScale = 1.0F;

    [Export]
    public float growSize = 2.0F;
    
    [Export]
    public float smallSize = 0.5F;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    private void SetScale(float s){
        this.Scale = new Vector2(s, s);
    }
    public void Grow() {
        if (this.currentScale < NORMAL_SIZE){
            this.currentScale = NORMAL_SIZE;
        } else {
            this.currentScale = this.growSize;
        }
        SetScale(currentScale);
    }
    
    public void Shrink() {
        if (this.currentScale > NORMAL_SIZE){
            this.currentScale = NORMAL_SIZE;
        } else {
            this.currentScale = this.smallSize;
        }
        SetScale(currentScale);
    }


    public void GotHitByBullet(Bullet bullet){
        if (bullet.GetBulletType() == Bullet.BulletTypeEnum.GROWER) {
            this.Grow();
        } else {
            this.Shrink();
        }
        // Invoke "setPosition" that checks for collisions
        SetPosition(this.Position.x, this.Position.y);
    }

    public void SetPosition(float x, float y) {
        Vector2 newPos = new Vector2(x, y);
        KinematicCollision2D collision = MoveAndCollide(newPos);
        this.Position = newPos;
        if (collision != null) OnCollision(collision.GetCollider());
    }
    public void OnCollision(Godot.Object body) {
        GD.Print("collision with " + body.GetType());
        Bullet objectAsBullet = body as Bullet;
        if (objectAsBullet == null){
            GD.Print("-- Object was not a bullet. Self-destroying");
            // Collisions with anything (except bullets, ironically) = death
            GetParent().RemoveChild(this);
            QueueFree();
        }else{
            GD.Print("Object was bullet");
        }
        
    }
}
