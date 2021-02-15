using GXPEngine.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine.Enemies
{
    class FastEnemy : Enemy
    {
        public FastEnemy(float spawnX, float spawnY, GameObject newTarget) : base("EnemyFast.png", "EnemyDefaultHitbox.png", spawnX, spawnY, 5, 4, 0, -30, newTarget, new FastProjectile())
        {
            moveSpeed = 2f;
            distanceFromTarget = 400f;
            shotCooldown = 2000;
            scoreWorth = 2;

            animationFrameTime = 10; //The amount of frames each animation frame should display for

            shootAnimationTime = 20;
            shootAnimationStartFrame = 0; //The starting frame of the shoot animation
            shootAnimationFrameCount = 6; //The length of the shoot animation in frames
            shootFrame = 4; //The frame at which a projectile is shot

            walkAnimationStartFrame = 7;
            walkAnimationFrameCount = 5;

            deathAnimationStartFrame = 12;
            deathAnimationFrameCount = 6;
            deathFrame = 17;
        }
    }
}
