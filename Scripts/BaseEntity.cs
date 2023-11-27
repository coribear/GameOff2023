using Godot;
using System;

public class BaseEntity : KinematicBody2D {
    private static float GROW_SPEED = 3.0F;
    protected Vector2 velocity = new Vector2(0.0F, 0.0F);
    protected Growable growableScale;
    [Export]
    public float growSize = 2.0F;
    [Export]
    public float smallSize = 0.5F;

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    private void SetScale(float s){
        this.Scale = new Vector2(s, s);
    }

    protected Vector2 GetSpriteSize(String spriteNodeName){
        AnimatedSprite sprite = GetNode<AnimatedSprite>("AnimatedSprite");
        Texture tex = sprite.Frames.GetFrame(sprite.Animation, sprite.Frame);
        return tex.GetSize();
    }
    public void MoveTo(float x, float y) {
        Vector2 movement = new Vector2(x, y) - this.Position;
        KinematicCollision2D collision = MoveAndCollide(movement);
        if (collision != null) OnMoveCollision(collision.Collider);
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

    public Growable GetGrowableScale(){
        return this.growableScale;
    }

    private void CreateGrowableWithCurrentSettings(){
        this.growableScale = new Growable(smallSize, this.Scale.x, growSize);
    }

    public override void _PhysicsProcess(float delta){
        // Nothing to do if we still don't have an instance of our growth tracker.
        // This should be done at _Ready, but for some reason it wasn't being called
        // for BaseEntity, so we are forced to create the instance of Growable here
        // if it doesn't exist
        if (this.growableScale == null) CreateGrowableWithCurrentSettings();
    
        // Let's use X as reference
        float scaleDiff = this.growableScale.GetScale() - this.Scale.x;
        SetScale(this.Scale.x + scaleDiff*delta*GROW_SPEED);
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
