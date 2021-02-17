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

            projectileType = ProjectileType.FAST;

            hitboxXOffset = 0;
            hitboxYOffset = 22;

            SetHitbox();
            SetAnimation("ProjectileFast.png", 3, 1, 3);
        }

        public override Projectile Duplicate(GameObject newSource, GameObject newTarget)
        {
            FastProjectile projectile = new FastProjectile();
            projectile.source = newSource;
            projectile.target = newTarget;
            return projectile;
        }
    }
}
