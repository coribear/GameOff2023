using Godot;
using System;

public class StaticObject : StaticBody2D {
    private static float GRAVITY = 150.0F;
    private static float GROW_SPEED = 3.0F;
    private Growable growableScale;
    [Export]
    public float growSize;
    [Export]
    public float smallSize;

    [Export]
    public bool applyGravity = false;
    private Vector2 curSpeed = new Vector2(0, 0); // This object doesn't move on its own,
                                                  // but it's affected by gravity

    public Growable GetGrowableScale(){
        return this.growableScale;
    }

    private void SetScale(float s){
        this.Scale = new Vector2(s, s);
    }

    private void CreateGrowableWithCurrentSettings(){
        this.growableScale = new Growable(smallSize, this.Scale.x, growSize);
    }
    public override void _Process(float delta){
        if (this.growableScale == null) CreateGrowableWithCurrentSettings();

        // Keep track of our growable object reference
        float scaleDiff = this.growableScale.GetScale() - this.Scale.x;
        SetScale(this.Scale.x + scaleDiff*delta*GROW_SPEED);

        if (this.applyGravity){
            this.curSpeed.y += GRAVITY*delta;
        }
        Vector2 newPos = this.Position;
        newPos.y += this.curSpeed.y*delta;
        newPos.x += this.curSpeed.x*delta;
        this.Position = newPos;
    }
}
