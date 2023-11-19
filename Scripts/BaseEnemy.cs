using Godot;
using System;

public class BaseEnemy : BaseEntity {
    private bool hasEnteredScreen = false;
    public override void _Ready() {
    }

    public override void _Process(float delta) {
        if (checkIfInsideViewArea()) {
            GD.Print("Enemy visible");
           this.hasEnteredScreen = true;
           DoMovement(delta);
        }else {
            // Not visible?
            this.velocity = new Vector2(0, 0);
            if (this.hasEnteredScreen) {
                GD.Print("Exiting stage");
                RemoveSelf();
            }
        }
    }

    // Must be overriden with enemy logic
    public virtual void DoMovement(float delta){
    }

    protected bool checkIfInsideViewArea(){
        // Get a reference to the parallax background object that we are attached to
        Stage stageNode = GetParent() as Stage;
        float diffX = this.Position.x + stageNode.Position.x;
        float diffY = this.Position.y + stageNode.Position.y;
        Vector2 spriteSize = GetSpriteSize("AnimatedSprite");
        float w2 = spriteSize.x/2;
        float h2 = spriteSize.y/2;
        //GD.Print(this.Position.x + "/" + stageNode.Position.x + "-> " + diffX + "[" + GetViewportRect() + "]");
        bool xVisible = (diffX + w2 >= 0) && (diffX - w2 < GetViewportRect().Size.x);
        bool yVisible = (diffY + h2 >= 0) && (diffY - h2 < GetViewportRect().Size.y);
        return xVisible && yVisible;
    }

    private void RemoveSelf(){
        GetParent().RemoveChild(this);
        QueueFree();
    }
    public override void OnCollision(Godot.Object body) {
        GD.Print("Enemy collided with " + body.GetType());
        StaticBody2D objectAsStaticBody = body as StaticBody2D;
        if (objectAsStaticBody != null){
            GD.Print("-- Object was a static body. Self-destroying");
            // Collisions with anything (except bullets, ironically) = death
            RemoveSelf();
        }
    }
}
