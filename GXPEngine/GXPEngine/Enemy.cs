using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    class Enemy : Sprite
    {
        private float moveSpeed = 2f; //The move speed of the enemy
        private float moveUntilDistance = 200f; //The enemy moves towards a target until this distance away

        private int chargeStartTime; //The last time the enemy started charging a projectile
        private int chargeDuration = 500; //The time it takes to charge
        private int projectileShotTime; //The last time the enemy shot a projectile
        private int chargeCooldown = 1000; //The cooldown (after shooting) before the enemy can charge again
        private bool isCharging; //Whether the enemy is currently charging

        private Projectile projectileToCharge; //The projectile that's charging

        private Transformable target; //The target the enemy is following

        //The position in Vec2
        private Vec2 position
        {
            get
            {
                return new Vec2(x, y);
            }
            set
            {
                x = value.x;
                y = value.y;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="spawnX">The X coordinates to spawn the enemy at</param>
        /// <param name="spawnY">The Y coordinates tp spawn the enemy at</param>
        /// <param name="newTarget">The target the enemy will follow and shoot at</param>
        public Enemy(float spawnX, float spawnY, Transformable newTarget) : base("triangle.png")
        {
            Initialize(spawnX, spawnY, newTarget);
        }

        private void Initialize(float spawnX, float spawnY, Transformable newTarget)
        {
            SetOrigin(width / 2, height / 2); //Center the origin of the enemy
            SetXY(spawnX, spawnY); //Set the X and Y position
            target = newTarget; //Set the target
        }

        public void Update()
        {
            MoveTowards(target);

            //If the enemy is already charging
            if (isCharging)
                ChargeCurrentProjectile();
            //Else if the charge cooldown is done
            else if (Time.now >= projectileShotTime + chargeCooldown)
                ChargeNewProjectile();
        }

        /// <summary>
        /// Move the enemy towards the designated target
        /// </summary>
        /// <param name="moveTarget">The target to move towards</param>
        private void MoveTowards(Transformable moveTarget)
        {
            Vec2 targetPosition = new Vec2(moveTarget.x, moveTarget.y); //Get the position of the target as a Vec2

            //If the distance to the target is large enough to still move
            if (DistanceTo(moveTarget) > moveUntilDistance)
            {
                Vec2 moveDirection = targetPosition - position; //Calculate the direction to move in
                moveDirection.Normalize(); //Normalize the move direction

                position += moveDirection * moveSpeed; //Move 'moveSpeed' units towards the move direction
            }
        }

        /// <summary>
        /// Start charging a new projectile
        /// </summary>
        private void ChargeNewProjectile()
        {
            isCharging = true; //Set state to charging

            projectileToCharge = new Projectile(this); //Spawn a new projectile

            chargeStartTime = Time.now; //Remember at which time the enemy started charging

            projectileToCharge.RotateTowards(target); //Rotate the projectile towards the target
        }

        /// <summary>
        /// Continue charging a projectile
        /// </summary>
        private void ChargeCurrentProjectile()
        {
            projectileToCharge.RotateTowards(target); //Rotate the projectile towards the target

            //If the enemy has charged for a sufficient amount of time
            if (Time.now >= chargeStartTime + chargeDuration)
                ShootProjectileTowards(target); //Shoot the projectile
        }

        /// <summary>
        /// Shoot the charging projectile towards the target
        /// </summary>
        /// <param name="shootTarget">The target to shoot the projectile at</param>
        private void ShootProjectileTowards(Transformable shootTarget)
        {
            isCharging = false; //Set state to not charging

            projectileShotTime = Time.now; //Remember the time the projectile was shot at

            Vec2 targetLocation = new Vec2(shootTarget.x, shootTarget.y); //Get the location of the target as a Vec2

            Vec2 targetRotation = targetLocation - position; //Get the rotation to shoot the projectile in

            projectileToCharge.Shoot(targetRotation); //Shoot the projectile in the target's direction
        }
    }
}
