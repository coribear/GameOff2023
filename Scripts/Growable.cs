using Godot;
using System;

public class Growable {
    private float normalSize;
    private float smallSize;
    private float bigSize;

    private float setScale;

    public enum ScaleSetting {
        NORMAL,
        SMALL,
        BIG
    };

    public Growable(){
        // Defaults
        SetSizes(0.5F, 1.0F, 2.0F);
        SetScale(this.normalSize);
    }
    
    public Growable(float smallSize, float normalSize, float bigSize){
        SetSizes(smallSize, normalSize, bigSize);
        SetScale(normalSize);
    }

    public void SetScale(float s){
        this.setScale = s;
    }

    public float GetScale(){
        return this.setScale;
    }

    public ScaleSetting GetCurrentScaleSetting(){
        if (this.setScale < normalSize) return ScaleSetting.SMALL;
        if (this.setScale > normalSize) return ScaleSetting.BIG;
        return ScaleSetting.NORMAL;
    }
    public void SetSizes(float smallSize, float normalSize, float bigSize){
        this.smallSize = smallSize;
        this.normalSize = normalSize;
        this.bigSize = bigSize;
    }

    public void Grow() {
        if (GetCurrentScaleSetting() == ScaleSetting.SMALL){
            this.setScale = normalSize;
        } else {
            this.setScale = this.bigSize;
        }
    }

    public void Shrink() {
        if (GetCurrentScaleSetting() == ScaleSetting.BIG){
            this.setScale = normalSize;
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
}
