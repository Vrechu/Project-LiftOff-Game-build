﻿using GXPEngine.Core;
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
        //========== OVERRIDEABLE ==========
        #region
        protected float moveSpeed = 2f; //The move speed of the enemy
        protected float distanceFromTarget = 400f; //The enemy moves towards a target until they are this distance away
        protected int shotCooldown = 2000; //The cooldown (after shooting) before the enemy can shoot again
        protected int scoreWorth = 1; //The amount of points the enemy is worth

        protected int hitboxXOffset = 0; //The X offset of the hitbox
        protected int hitboxYOffset = 0; //The Y offset of the hitbox

        protected int projectileLauncherXOffset = 0; //The X offset of the projectile launcher
        protected int projectileLauncherYOffset = 0; //The Y offset of the projectile launcher

        protected byte animationFrameTime = 10; //The amount of frames each animation frame should display for

        protected int idleAnimationStartFrame = 0; //The starting frame of the idle animation
        protected int idleAnimationFrameCount = 1; //The number of frames the idle animation plays for

        protected byte shootAnimationTime = 10; //The amount of frames each frame is shown for during shooting
        protected int shootAnimationStartFrame = 0; //The starting frame of the shoot animation
        protected int shootAnimationFrameCount = 6; //The length of the shoot animation in frames
        protected int shootFrame = 4; //The frame at which a projectile is shot

        protected int walkAnimationStartFrame = 7; //The starting frame of the walking animation
        protected int walkAnimationFrameCount = 5; //The number of frames the walking animation plays for

        protected int deathAnimationStartFrame = 12; //The starting frame of the death animation
        protected int deathAnimationFrameCount = 6; //The number of frames the death animation plays for
        protected int deathFrame = 17; //The frame the enemy dies at

        protected EnemyType enemyType = EnemyType.SLOW; //The type of the enemy
        #endregion

        //============= EVENTS =============
        #region
        public static event Action OnEnemyDestroyed; //When the enemy is destroyed
        public static event Action<EnemyType> OnHit; //When the enemy is hit
        #endregion

        private int projectileShotTime; //The last time the enemy shot a projectile
        private bool isFiring = false; //Whether the enemy is firing
        private bool isDying = false; //Whether the enemy is dying
        private bool canMove = true; //Whether the enemy can move

        private EnemyState enemyState = EnemyState.IDLE; //The state of the enemy

        private ProjectileLauncher projectileManager; //The projectile that's charging
        private EnemyAnimation enemyAnimation; //The animation of the enemy
        private Sprite dropShadow; //The shadow of the enemy

        private GameObject target; //The target the enemy is following
        private LineOfSight lineOfSight; //The line of sight of the enemy

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
        /// <param name="spawnX">The X coordinates to spawn at</param>
        /// <param name="spawnY">The Y coordinates to spawn at</param>
        /// <param name="newTarget">The target the enemy will follow</param>
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

            GameManager.OnPlayerDeath += StopMoving;

            name = "Enemy";
        }

        /// <summary>
        /// Sets the hitbox of the enemy
        /// </summary>
        /// <param name="hitboxXOffset">The X offset of the hitbox</param>
        /// <param name="hitboxYOffset">The Y offset of the hitbox</param>
        protected void SetHitbox(int hitboxXOffset, int hitboxYOffset)
        {
            SetOrigin(width / 2 + hitboxXOffset, height / 2 + hitboxYOffset); //Center the origin of the enemy
        }

        /// <summary>
        /// Sets the projectile launcher of the enemy
        /// </summary>
        /// <param name="projectileXOffset">The X offset of the launcher</param>
        /// <param name="projectileYOffset">The Y offset of the launcher</param>
        /// <param name="projectile">The projectile the launcher will shoot</param>
        protected void SetProjectileLauncher(int projectileXOffset, int projectileYOffset, Projectile projectile)
        {
            projectileManager = new ProjectileLauncher(projectile, this, target);
            projectileManager.SetXY(-projectileXOffset, projectileYOffset);
            enemyAnimation.AddChild(projectileManager);
        }

        /// <summary>
        /// Sets the animation of the enemy
        /// </summary>
        /// <param name="sprite">The spritesheet for the animation</param>
        /// <param name="cols">The number of columns the spritesheet has</param>
        /// <param name="rows">The number of rows the spritesheet has</param>
        protected void SetAnimation(string sprite, int cols, int rows)
        {
            enemyAnimation = new EnemyAnimation(sprite, cols, rows);
            AddChild(enemyAnimation);
        }

        /// <summary>
        /// Sets the shadow for the enemy
        /// </summary>
        /// <param name="shadowSprite">The sprite of the shadow</param>
        /// <param name="spriteWidth">The width of the shadow</param>
        /// <param name="spriteHeight">The height of the shadow</param>
        /// <param name="xOffset">The X offset of the shadow</param>
        /// <param name="yOffset">The Y offset of the shadow</param>
        protected void SetDropShadow(string shadowSprite, int spriteWidth, int spriteHeight, int xOffset, int yOffset)
        {
            dropShadow = new Sprite(shadowSprite);
            dropShadow.color = 0x000000;
            dropShadow.SetOrigin(dropShadow.width / 2, dropShadow.height / 2);
            dropShadow.width = spriteWidth;
            dropShadow.height = spriteHeight;
            dropShadow.SetXY(xOffset, yOffset);
            AddChild(dropShadow);
        }

        public void Update()
        {
            if (isDying)
            {
                Dying();
            }
            else if (canMove)
            {
                lineOfSight.RotateTowards(RotationTowards(target)); //Rotate the line of sight towards the target
                lineOfSight.SetLength(DistanceTo(target)); //Set the length of the line of sight to match the distance between the enemy and target

                //If the line of sight is colliding with an enemy that's not its source
                if (lineOfSight.CollidingWithEnemy)
                {
                    RotateAround(target); //Rotate around the target
                }

                //If the enemy is firing
                if (isFiring)
                {
                    ChargeShot();
                }
                //Otherwise if the enemy can move
                else if (DistanceTo(target) > distanceFromTarget)
                {
                    MoveInDirection(RotationTowards(target)); //Move towards the target
                }
                //Otherwise if the enemy is idle
                else
                {
                    enemyState = EnemyState.IDLE;
                }

                //If the cooldown has run out
                if (Time.now > projectileShotTime + shotCooldown && !lineOfSight.CollidingWithEnemy)
                {
                    StartFiring(); //Charge a new projectile
                }

                //Face the enemy the correct way
                if (target.x > x)
                {
                    FaceLeft(false);
                }
                else
                {
                    FaceLeft(true);
                }
            }

            SetAnimationCycle(enemyState);
        }

        /// <summary>
        /// Starts firing a projectile
        /// </summary>
        private void StartFiring()
        {
            enemyState = EnemyState.SHOOTING;
            isFiring = true;
        }

        /// <summary>
        /// Stops firing a projectile
        /// </summary>
        private void StopFiring()
        {
            isFiring = false;
        }

        /// <summary>
        /// Run while the enemy is charging a shot
        /// </summary>
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
            enemyState = EnemyState.WALKING;
            moveDirection.Normalize();
            MoveUntilCollision(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed, dropShadow, game.FindObjectsOfType<Wall>()); //Move 'moveSpeed' units towards the move direction
            //Position += moveDirection * moveSpeed; 
        }

        /// <summary>
        /// Faces the enemy left or right
        /// </summary>
        /// <param name="faceLeft">Whether the enemy should face left (or right)</param>
        private void FaceLeft(bool faceLeft)
        {
            switch (faceLeft)
            {
                case true:
                    enemyAnimation.width = Mathf.Abs(enemyAnimation.width);
                    dropShadow.x = Mathf.Abs(dropShadow.x);
                    break;
                case false:
                    enemyAnimation.width = -Mathf.Abs(enemyAnimation.width);
                    dropShadow.x = -Mathf.Abs(dropShadow.x);
                    break;
            }
        }

        /// <summary>
        /// Stop the enemy from moving
        /// </summary>
        private void StopMoving()
        {
            canMove = false;
            enemyState = EnemyState.IDLE;
        }

        /// <summary>
        /// Set the animation cycle for the enemy animation
        /// </summary>
        /// <param name="newEnemyState"></param>
        private void SetAnimationCycle(EnemyState newEnemyState)
        {
            switch (newEnemyState)
            {
                case EnemyState.IDLE:
                    enemyAnimation.SetAnimationCycle(idleAnimationStartFrame, idleAnimationFrameCount, animationFrameTime);
                    break;
                case EnemyState.WALKING:
                    enemyAnimation.SetAnimationCycle(walkAnimationStartFrame, walkAnimationFrameCount, animationFrameTime);
                    break;
                case EnemyState.SHOOTING:
                    enemyAnimation.SetAnimationCycle(shootAnimationStartFrame, shootAnimationFrameCount, shootAnimationTime);
                    break;
                case EnemyState.DYING:
                    enemyAnimation.SetAnimationCycle(deathAnimationStartFrame, deathAnimationFrameCount, animationFrameTime);
                    break;
            }
        }

        /// <summary>
        /// Kills the enemy
        /// </summary>
        private void Die()
        {
            isDying = true;
            GameManager.Singleton._playerScore += scoreWorth;
            OnHit?.Invoke(enemyType);
        }

        /// <summary>
        /// While the enemy is dying
        /// </summary>
        private void Dying()
        {
            enemyState = EnemyState.DYING;
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
                if (projectile.HasLeftSource || projectile.IsLethal)
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

        protected override void OnDestroy()
        {
            GameManager.OnPlayerDeath -= StopMoving;
            OnEnemyDestroyed?.Invoke();
            base.OnDestroy();
        }
    }
}
