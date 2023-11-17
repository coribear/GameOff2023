using Godot;
using System;

public class BaseEntity : KinematicBody2D {
    const float NORMAL_SIZE = 1.0F;
    private float setScale = 1.0F;
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
        if (this.setScale < NORMAL_SIZE){
            this.setScale = NORMAL_SIZE;
        } else {
            this.setScale = this.growSize;
        }
    }
    
    public void Shrink() {
        if (this.setScale > NORMAL_SIZE){
            this.setScale = NORMAL_SIZE;
        } else {
            this.setScale = this.smallSize;
        }
    }


    public void GotHitByBullet(Bullet bullet){
        if (bullet.GetBulletType() == Bullet.BulletTypeEnum.GROWER) {
            this.Grow();
        } else {
            this.Shrink();
        }
    }

    public void MoveTo(float x, float y) {
        Vector2 movement = new Vector2(x, y) - this.Position;
        KinematicCollision2D collision = MoveAndCollide(movement);
        if (collision != null) OnMoveCollision(collision.GetCollider());
    }
    private void OnMoveCollision(Godot.Object body) {
        // Collision when we move. We will invoke both the target OnCollision (if they are a BaseEntity object)
        // and our own.
        BaseEntity bodyAsBaseEntity = body as BaseEntity;
        if (bodyAsBaseEntity != null) bodyAsBaseEntity.OnCollision(this);

        this.OnCollision(body);
    }

    public virtual void OnCollision(Godot.Object body) {
    }

    public override void _PhysicsProcess(float delta){
        // Let's use X as reference
        float scaleDiff = this.setScale - this.Scale.x;
        SetScale(this.Scale.x + scaleDiff*0.1F);
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
