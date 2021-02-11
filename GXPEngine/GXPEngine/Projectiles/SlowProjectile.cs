using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine.Projectiles
{
    class SlowProjectile : Projectile
    {
        public SlowProjectile() : base("ProjectileSlow.png")
        {
            //Override the variables
            moveSpeed = 5f;
        }

        public override Projectile Duplicate(GameObject newSource)
        {
            SlowProjectile projectile = new SlowProjectile();
            projectile.source = newSource;
            return projectile;
        }
    }
}
