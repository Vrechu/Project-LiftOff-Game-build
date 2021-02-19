using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Enemies;

class Wall : Sprite
{
    public Wall(float px, float py, int pwidth, int pheight) : base("square.png")
    {
        x = px;
        y = py;
        width = pwidth;
        height = pheight;
        alpha = 0;
    }

    // collision detection with projectiles
    public void OnCollision(GameObject other)
    {
        if(!(other is AnimationSprite) && other.parent is Projectile projectile) //if other is no animationsprite and child of a projectile
        {
            if (!projectile.InWall || projectile.MoveDirection.y < 0) // if projectile is not in wall or the movedirection is up.
            {
                projectile.StartExploding(); // explode
            }
        }
    }
}

