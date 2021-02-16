using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine.Projectiles
{
    class HomingProjectile : Projectile
    {
        public HomingProjectile() : base("ProjectileHomingHitbox.png")
        {
            moveSpeed = 5f;

            shootAnimationStartFrame = 0;
            shootAnimationFrameCount = 3;

            explosionAnimationFrame = 0;

            hitboxXOffset = 1;
            hitboxYOffset = 1;

            SetHitbox();
            SetAnimation("ProjectileHoming.png", 2, 2, 3);
        }

        public override Projectile Duplicate(GameObject newSource)
        {
            HomingProjectile projectile = new HomingProjectile();
            projectile.source = newSource;
            return projectile;
        }
    }
}
