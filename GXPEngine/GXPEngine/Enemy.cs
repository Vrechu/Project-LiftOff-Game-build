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
        private float moveUntilDistance = 400f; //The enemy moves towards a target until they are this distance away

        private int projectileShotTime; //The last time the enemy shot a projectile
        private int shotCooldown = 1500; //The cooldown (after shooting) before the enemy can shoot again

        private Projectile projectileToCharge; //The projectile that's charging

        private Transformable target; //The target the enemy is following

        //The position in Vec2
        private Vec2 Position
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
            name = "Enemy";
        }

        public void Update()
        {
            MoveTowards(target); //Move towards the target

            //If the cooldown has run out
            if (Time.now > projectileShotTime + shotCooldown)
                ChargeNewProjectile(); //Charge a new projectile
        }

        private void ChargeNewProjectile()
        {
            projectileShotTime = Time.now;
            projectileToCharge = new Projectile(this.x, this.y, this, target);
            projectileToCharge.RotateTowardsObject(target);
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
                Vec2 moveDirection = targetPosition - Position; //Calculate the direction to move in
                moveDirection.Normalize(); //Normalize the move direction

                Position += moveDirection * moveSpeed; //Move 'moveSpeed' units towards the move direction
            }
        }

        public void OnCollision(GameObject other)
        {
            if(other is Projectile)
            {
                Projectile projectile = other as Projectile;
                if (projectile.HasLeftSource)
                {
                    LateDestroy();
                    projectile.LateDestroy();
                    projectileToCharge.LateDestroy();
                }
            }
        }
    }
}
