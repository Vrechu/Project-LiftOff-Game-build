using GXPEngine.Enemies;
using GXPEngine.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    class SlowEnemy : Enemy
    {
        public SlowEnemy(float spawnX, float spawnY, GameObject newTarget) : base("EnemyDefaultHitbox.png", spawnX, spawnY, newTarget)
        {
            moveSpeed = 2f;
            distanceFromTarget = 400f;
            shotCooldown = 2000;
            scoreWorth = 1;

            hitboxXOffset = 0;
            hitboxYOffset = -30;

            projectileLauncherXOffset = 32;
            projectileLauncherYOffset = 15;

            animationFrameTime = 10; //The amount of frames each animation frame should display for

            shootAnimationTime = 10;
            shootAnimationStartFrame = 0; //The starting frame of the shoot animation
            shootAnimationFrameCount = 6; //The length of the shoot animation in frames
            shootFrame = 4; //The frame at which a projectile is shot

            walkAnimationStartFrame = 7;
            walkAnimationFrameCount = 5;

            deathAnimationStartFrame = 12;
            deathAnimationFrameCount = 6;
            deathFrame = 17;

            enemyType = EnemyType.SLOW;

            SetHitbox(hitboxXOffset, hitboxYOffset);
            SetDropShadow("DropShadow.png", 119, 25, 10, 55);
            SetAnimation("EnemySlow.png", 5, 4);
            SetProjectileLauncher(projectileLauncherXOffset, projectileLauncherYOffset, new SlowProjectile());
        }
    }
}
