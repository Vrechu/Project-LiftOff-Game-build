using GXPEngine.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine.Enemies
{
    class HomingEnemy : Enemy
    {
        public HomingEnemy(float spawnX, float spawnY, GameObject newTarget) : base("EnemyHomingHitbox.png", spawnX, spawnY, newTarget)
        {
            moveSpeed = 2f;
            distanceFromTarget = 400f;
            shotCooldown = 2000;
            scoreWorth = 2;

            hitboxXOffset = 3;
            hitboxYOffset = -20;

            projectileLauncherXOffset = 32;
            projectileLauncherYOffset = 15;

            animationFrameTime = 10; //The amount of frames each animation frame should display for

            shootAnimationTime = 20;
            shootAnimationStartFrame = 0; //The starting frame of the shoot animation
            shootAnimationFrameCount = 4; //The length of the shoot animation in frames
            shootFrame = 2; //The frame at which a projectile is shot

            walkAnimationStartFrame = 4;
            walkAnimationFrameCount = 4;

            deathAnimationStartFrame = 9;
            deathAnimationFrameCount = 7;
            deathFrame = 15;

            SetHitbox(hitboxXOffset, hitboxYOffset);
            SetDropShadow("DropShadow.png", 119, 25, 10, 55);
            SetAnimation("EnemyHoming.png", 4, 4);
            SetProjectileLauncher(projectileLauncherXOffset, projectileLauncherYOffset, new HomingProjectile());
        }
    }
}
