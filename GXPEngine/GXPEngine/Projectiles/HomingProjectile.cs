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

            projectileType = ProjectileType.HOMING;

            hitboxXOffset = 1;
            hitboxYOffset = 1;

            shouldHome = true;

            SetHitbox();
            SetShadow("DropShadow.png", 64, 15);
            SetAnimation("ProjectileHoming.png", 2, 2, 3);
        }

        public override Projectile Duplicate(GameObject newSource, GameObject newTarget)
        {
            HomingProjectile projectile = new HomingProjectile();
            projectile.source = newSource;
            projectile.target = newTarget;
            return projectile;
        }
    }
}
