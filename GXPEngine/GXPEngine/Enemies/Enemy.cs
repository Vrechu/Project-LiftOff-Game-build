using GXPEngine.Core;
using GXPEngine.Enemies;
using GXPEngine.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace GXPEngine
{
    abstract class Enemy : Sprite
    {
        protected float moveSpeed = 2f; //The move speed of the enemy
        protected float distanceFromTarget = 400f; //The enemy moves towards a target until they are this distance away
        protected int shotCooldown = 2000; //The cooldown (after shooting) before the enemy can shoot again
        protected int scoreWorth = 1; //The amount of points the enemy is worth

        protected int hitboxXOffset = 0;
        protected int hitboxYOffset = 0;

        protected int projectileLauncherXOffset = 0;
        protected int projectileLauncherYOffset = 0;

        protected byte animationFrameTime = 10; //The amount of frames each animation frame should display for

        protected byte shootAnimationTime = 10; //The amount of frames each frame is shown for during shooting
        protected int shootAnimationStartFrame = 0; //The starting frame of the shoot animation
        protected int shootAnimationFrameCount = 6; //The length of the shoot animation in frames
        protected int shootFrame = 4; //The frame at which a projectile is shot

        protected int walkAnimationStartFrame = 7;
        protected int walkAnimationFrameCount = 5;

        protected int deathAnimationStartFrame = 12;
        protected int deathAnimationFrameCount = 6;
        protected int deathFrame = 17;

        private int projectileShotTime; //The last time the enemy shot a projectile
        private bool isFiring = false;
        private bool isDying = false;

        private ProjectileLauncher projectileManager; //The projectile that's charging
        private EnemyAnimation enemyAnimation;

        private GameObject target; //The target the enemy is following
        private LineOfSight lineOfSight;

        //The position in Vec2
        private Vec2 Position
        {
            get
            {
                Vector2 newVector = TransformPoint(0, 0);
                return new Vec2(newVector.x, newVector.y);
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
        /// <param name="hitboxSprite">The sprite of the hitbox</param>
        /// <param name="spawnX">The X coordinate to spawn at</param>
        /// <param name="spawnY">The Y coordinate to spawn at</param>
        /// <param name="newTarget">The target of the enemy</param>
        public Enemy(string hitboxSprite, float spawnX, float spawnY, GameObject newTarget) : base(hitboxSprite)
        {
            Initialize(spawnX, spawnY, newTarget);
        }

        private void Initialize(float spawnX, float spawnY, GameObject newTarget)
        {
            color = 0x00ff06;
            alpha = 0;

            SetXY(spawnX, spawnY); //Set the X and Y position
            target = newTarget; //Set the target

            game.AddChild(this);

            //Create a new line of sight
            lineOfSight = new LineOfSight(this);
            AddChild(lineOfSight);

            name = "Enemy";
        }

        protected void SetHitbox(int hitboxXOffset, int hitboxYOffset)
        {
            Vector2 hitboxOffset = new Vector2(hitboxXOffset / scale, hitboxYOffset * scale);
            SetOrigin(width / 2 + hitboxXOffset, height / 2 + hitboxYOffset); //Center the origin of the enemy
        }

        protected void SetProjectileLauncher(int projectileXOffset, int projectileYOffset, Projectile projectile)
        {
            projectileManager = new ProjectileLauncher(projectile, this);
            projectileManager.SetXY(-projectileXOffset, projectileYOffset);
            enemyAnimation.AddChild(projectileManager);
        }

        protected void SetAnimation(string sprite, int cols, int rows)
        {
            enemyAnimation = new EnemyAnimation(sprite, cols, rows);
            AddChild(enemyAnimation);
        }

        public void Update()
        {
            if (isDying)
            {
                Dying();
            }
            else
            {
                lineOfSight.RotateTowards(RotationTowards(target)); //Rotate the line of sight towards the target
                lineOfSight.SetLength(DistanceTo(target)); //Set the length of the line of sight to match the distance between the enemy and target

                //If the line of sight is colliding with an enemy that's not its source
                if (lineOfSight.CollidingWithEnemy)
                {
                    RotateAround(target); //Rotate around the target
                }

                if (isFiring)
                {
                    ChargeShot();
                }
                //If the distance to the target is large enough to still move && is not firing
                else if (DistanceTo(target) > distanceFromTarget)
                {
                    MoveInDirection(RotationTowards(target)); //Move towards the target
                }
                else
                {
                    enemyAnimation.SetAnimationCycle(0, 1, animationFrameTime);
                }

                //If the cooldown has run out
                if (Time.now > projectileShotTime + shotCooldown && !lineOfSight.CollidingWithEnemy)
                {
                    StartFiring(); //Charge a new projectile
                }

                if (target.x > x)
                {
                    FaceDirection(false);
                }
                else
                {
                    FaceDirection(true);
                }
            }
        }

        private void StartFiring()
        {
            enemyAnimation.SetAnimationCycle(shootAnimationStartFrame, shootAnimationFrameCount, shootAnimationTime);
            isFiring = true;
        }

        private void StopFiring()
        {
            isFiring = false;
        }

        private void ChargeShot()
        {
            if (enemyAnimation.currentFrame == shootFrame && Time.now > projectileShotTime + shotCooldown)
            {
                projectileManager.ShootAt(target);
                projectileShotTime = Time.now;
            }

            if (enemyAnimation.currentFrame == shootAnimationStartFrame + shootAnimationFrameCount - 1)
            {
                StopFiring();
            }
        }

        /// <summary>
        /// Move the enemy towards the designated target
        /// </summary>
        /// <param name="moveTarget">The target to move towards</param>
        private Vec2 RotationTowards(Transformable moveTarget)
        {
            Vector2 tempPosotion = moveTarget.TransformPoint(0, 0); //Get the position of the target as a Vec2
            Vec2 targetPosition = new Vec2(tempPosotion.x, tempPosotion.y);
            Vec2 moveDirection = targetPosition - Position; //Calculate the direction to move in
            moveDirection.Normalize(); //Normalize the move direction

            return moveDirection;
        }

        /// <summary>
        /// Rotate around an object
        /// </summary>
        /// <param name="rotationTarget">The object to rotate around</param>
        private void RotateAround(Transformable rotationTarget)
        {
            Vec2 newRotation = RotationTowards(rotationTarget);
            newRotation.RotateDegrees(90);
            MoveInDirection(newRotation);
        }

        /// <summary>
        /// Move in the designated direction
        /// </summary>
        /// <param name="moveDirection">The direction to move in</param>
        private void MoveInDirection(Vec2 moveDirection)
        {
            StopFiring();
            enemyAnimation.SetAnimationCycle(walkAnimationStartFrame, walkAnimationFrameCount, animationFrameTime);
            moveDirection.Normalize();
            Position += moveDirection * moveSpeed; //Move 'moveSpeed' units towards the move direction
        }

        private void FaceDirection(bool faceLeft)
        {
            switch (faceLeft)
            {
                case true: enemyAnimation.width = Mathf.Abs(enemyAnimation.width); break;
                case false: enemyAnimation.width = -Mathf.Abs(enemyAnimation.width); break;
            }
        }

        private void Die()
        {
            isDying = true;
            GameManager.Singleton._playerScore += scoreWorth;
            Console.WriteLine(GameManager.Singleton._playerScore);
        }

        private void Dying()
        {
            enemyAnimation.SetAnimationCycle(deathAnimationStartFrame, deathAnimationFrameCount, animationFrameTime);
            if (enemyAnimation.currentFrame == deathFrame)
            {
                LateDestroy();
            }
        }

        public void OnCollision(GameObject other)
        {
            if (other is Projectile && !isDying)
            {
                Projectile projectile = other as Projectile;
                if (projectile.HasLeftSource)
                {
                    Die();
                    projectile.StartExploding();
                }
            }
            else if (other is Enemy && !isDying)
            {
                Vec2 newRotation = RotationTowards(other);
                newRotation.RotateDegrees(180);
                MoveInDirection(newRotation);
            }
        }
    }
}
