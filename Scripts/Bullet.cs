using Godot;
using System;

public class Bullet : Area2D {
    public enum BulletTypeEnum{
        GROWER,
        SHRINKER
    };

    [Export]
    public float speed = 20.0F;

    [Signal]
    public delegate void Hit();

    private bool alive = false;
    private BulletTypeEnum bulletType = BulletTypeEnum.GROWER; 

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        SetAlive(false);
        GD.Print("Bullet spawn");
    }

    public void Spawn(float x, float y){
        this.Position = new Vector2(x, y);
        SetAlive(true);
    }

    public void Spawn(BulletTypeEnum bulletType, float x, float y){
        this.bulletType = bulletType;
        Spawn(x, y);
    }

    private void SetAlive(bool flag){
        this.alive = flag;
        this.Visible = this.alive;
    }

    public bool IsAlive(){
        return this.alive;
    }

    public BulletTypeEnum GetBulletType(){
        return this.bulletType;
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

    public void OnBulletBodyEntered(PhysicsBody2D body) {
        if (this.alive) {
            GD.Print("Bullet collided!");
            GD.Print(body.GetType());
            // Try to cast it as a BaseEntity
            BaseEntity bodyAsEntity = body as BaseEntity;
            StaticObject bodyAsStatic = body as StaticObject;
            if (bodyAsEntity != null && bodyAsEntity.GetGrowableScale() != null){
                GD.Print("Invoking hit event of [BaseEntity]");
                bodyAsEntity.GetGrowableScale().GotHitByBullet(this);
            }else if (bodyAsStatic != null && bodyAsStatic.GetGrowableScale() != null){
                GD.Print("Invoking hit event of [StaticObject]");
                bodyAsStatic.GetGrowableScale().GotHitByBullet(this);
            }
            SetAlive(false);
            EmitSignal(nameof(Hit));
        }
    }
}
