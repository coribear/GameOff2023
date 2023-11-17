using Godot;
using System;

public class BaseEntity : KinematicBody2D {
    const float NORMAL_SIZE = 1.0F;
    private float currentScale = 1.0F;
    protected Vector2 velocity = new Vector2(0.0F, 0.0F);

    [Export]
    public float growSize = 2.0F;
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
        MoveTo(this.Position.x, this.Position.y);
    }

    public void MoveTo(float x, float y) {
        Vector2 movement = new Vector2(x, y) - this.Position;
        KinematicCollision2D collision = MoveAndCollide(movement);
        //this.Position = newPos;
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

    public override void _PhysicsProcess(float delta){

        // Natural deacceleration
        velocity.x *= 0.9F;
        velocity.y *= 0.9F;

        if (Math.Abs(velocity.x) < 0.01) velocity.x = 0;
        if (Math.Abs(velocity.y) < 0.01) velocity.y = 0;

        // TO-DO: Limit velocity and range of motion
        float x = this.Position.x + velocity.x * delta;
        float y = this.Position.y + velocity.y * delta;
        MoveTo(x,y);        
    }
}
