using GXPEngine.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine.Enemies
{
    class FastEnemy : Enemy
    {
        public FastEnemy(float spawnX, float spawnY, GameObject newTarget) : base("EnemyDefaultHitbox.png", spawnX, spawnY, newTarget)
        {
            //========== OVERRIDE ==========
            #region
                moveSpeed = 2f;
                distanceFromTarget = 400f;
                shotCooldown = 2000;
                scoreWorth = 2;

                hitboxXOffset = 0;
                hitboxYOffset = -30;

                projectileLauncherXOffset = 32;
                projectileLauncherYOffset = 15;

                animationFrameTime = 10;

                shootAnimationTime = 20;
                shootAnimationStartFrame = 0;
                shootAnimationFrameCount = 6;
                shootFrame = 4;

                walkAnimationStartFrame = 7;
                walkAnimationFrameCount = 5;

                deathAnimationStartFrame = 12;
                deathAnimationFrameCount = 6;
                deathFrame = 17;

                enemyType = EnemyType.FAST;
            #endregion


            //Set the extra properties of the enemy
            SetHitbox(hitboxXOffset, hitboxYOffset);
            SetDropShadow("DropShadow.png", 119, 25, 10, 55);
            SetAnimation("EnemyFast.png", 5, 4);
            SetProjectileLauncher(projectileLauncherXOffset, projectileLauncherYOffset, new FastProjectile());
        }
    }
}
