﻿using System;
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

            explosionAnimationFrame = 2;

            hitboxXOffset = 0;
            hitboxYOffset = 8;

            SetHitbox();
            SetAnimation("ProjectileSlow.png", 3, 1, 3);
        }

        public override Projectile Duplicate(GameObject newSource)
        {
            SlowProjectile projectile = new SlowProjectile();
            projectile.source = newSource;
            return projectile;
        }
    }
}
