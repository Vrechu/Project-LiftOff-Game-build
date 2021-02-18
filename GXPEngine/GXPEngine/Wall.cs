using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

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

    public void OnCollision(GameObject other)
    {
        if(other is Projectile newProjectile)
        {
            newProjectile.StartExploding();
        }
    }
}

