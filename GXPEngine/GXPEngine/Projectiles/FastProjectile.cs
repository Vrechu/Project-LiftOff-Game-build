using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine.Projectiles
{
    class FastProjectile : Projectile
    {
        public FastProjectile() : base("ProjectileFastHitbox.png")
        {
            moveSpeed = 10f;

            shootAnimationStartFrame = 0;
            shootAnimationFrameCount = 2;

            explosionAnimationFrame = 2;

            SetHitbox(0, -22);
            SetAnimation("ProjectileFast.png", 3, 1, 3);
        }

        public override Projectile Duplicate(GameObject newSource)
        {
            FastProjectile projectile = new FastProjectile();
            projectile.source = newSource;
            return projectile;
        }
    }
}
