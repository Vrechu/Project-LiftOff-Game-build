using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine.Projectiles
{
    class FastProjectile : Projectile
    {
        public FastProjectile() : base("ProjectileFast.png", 3, 1, "ProjectileFastHitbox.png", 0, -22)
        {
            moveSpeed = 10f;
        }

        public override Projectile Duplicate(GameObject newSource)
        {
            FastProjectile projectile = new FastProjectile();
            projectile.source = newSource;
            return projectile;
        }
    }
}
