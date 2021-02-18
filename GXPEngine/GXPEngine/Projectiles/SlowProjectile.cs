using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine.Projectiles
{
    class SlowProjectile : Projectile
    {
        public SlowProjectile() : base("ProjectileSlowHitbox.png")
        {
            //Override the variables
            moveSpeed = 5f;

            shootAnimationStartFrame = 0;
            shootAnimationFrameCount = 2;

            projectileType = ProjectileType.SLOW;

            explosionAnimationFrame = 2;

            hitboxXOffset = 0;
            hitboxYOffset = 8;

            SetHitbox();
            SetShadow("DropShadow.png", 64, 18, 0xff0000);
            SetAnimation("ProjectileSlow.png", 3, 1, 3);
        }

        public override Projectile Duplicate(GameObject newSource, GameObject newTarget)
        {
            SlowProjectile projectile = new SlowProjectile();
            projectile.source = newSource;
            projectile.target = newTarget;
            return projectile;
        }
    }
}
