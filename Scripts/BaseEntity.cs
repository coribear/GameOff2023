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
    }
}
