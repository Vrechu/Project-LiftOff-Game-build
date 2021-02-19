using GXPEngine.Core;
using GXPEngine.Projectiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Policy;

namespace GXPEngine
{
    abstract class Projectile : Sprite
    {
        //========== OVERRIDEABLE ==========
        #region
            protected float moveSpeed = 5f; //The speed at which the projectile moves

            protected int shootAnimationStartFrame = 0; //The starting frame of the shooting animation
            protected int shootAnimationFrameCount = 2; //The number of frames the shooting animation has

            protected int hitboxXOffset = 0; //The X offset of the hitbox
            protected int hitboxYOffset = 0; //The Y offset of the hitbox

            protected int explosionAnimationFrame = 2; //The frame of the explosion
            private int explosionDuration = 200; //The time the explosion will show for (milliseconds)
            protected bool shouldHome = false; //Whether the projectile should be homing
        #endregion

        //============= EVENTS =============
        #region
            public static event Action<ProjectileType> OnShot; //When the projectile is shot
            public static event Action<ProjectileType> OnExplode; //When the projectile explodes
        #endregion

        private bool isExploding = false; //Whether the projectile is exploding
        private int explosionStartTime; //The time at which the projectile started exploding

        public ProjectileType projectileType = ProjectileType.SLOW; //The type of projectile

        public bool HasLeftSource { get; private set; } = false; //Whether the projectile has left its source
        protected GameObject source; //The source object that the projectile came from
        protected GameObject target; //The target the projectile aims for (if homing)
        public bool IsLethal { get; private set; } = false; //Whether the projectile is lethal to its source
        public bool InWall { get; private set; } = false; //Whether the projectile is inside a wall

        private AnimationSprite projectileAnimation; //The animation of the projectile
        private Sprite dropShadow; //The shadow of the projectile

        //The direction in which the projectile will move
        public Vec2 MoveDirection { get; private set; }

        //The position in Vector2
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
        /// <param name="hitboxSprite">The sprite for the hitbox</param>
        public Projectile(string hitboxSprite) : base(hitboxSprite)
        {
            Initialize();
        }

        private void Initialize()
        {
            GameManager.OnPlayerDeath += Dissapear;

            color = 0x00ff06;
            alpha = 0;

            name = "Projectile";
        }

        /// <summary>
        /// Sets the hitbox of the projectile
        /// </summary>
        protected void SetHitbox()
        {
            SetOrigin(width / 2, height / 2); //Set the origin
        }

        /// <summary>
        /// Sets the animation
        /// </summary>
        /// <param name="animationSprite">The animation spritesheet</param>
        /// <param name="spriteCols">The number of colums the spritesheet has</param>
        /// <param name="spriteRows">The number of rows the spritesheet has</param>
        /// <param name="spriteFrames">The number of frames the spritesheet has</param>
        protected void SetAnimation(string animationSprite, int spriteCols, int spriteRows, int spriteFrames)
        {
            projectileAnimation = new AnimationSprite(animationSprite, spriteCols, spriteRows, frames: spriteFrames);
            projectileAnimation.SetOrigin(width / 2 + hitboxXOffset, height / 2 + hitboxYOffset);

            projectileAnimation.SetCycle(shootAnimationStartFrame, shootAnimationFrameCount);

            AddChild(projectileAnimation);
        }

        /// <summary>
        /// Sets the shadow for the proejctile
        /// </summary>
        /// <param name="shadowSprite">The sprite of the shadow</param>
        /// <param name="spritewidth">The width of the shadow</param>
        /// <param name="spriteHeight">The height of the shadow</param>
        /// <param name="color">The color hue of the shadow</param>
        protected void SetShadow(string shadowSprite, int spritewidth, int spriteHeight, uint color)
        {
            dropShadow = new Sprite(shadowSprite);
            dropShadow.color = color;
            dropShadow.alpha = 0.5f;
            dropShadow.SetOrigin(dropShadow.width / 2, -dropShadow.height - 50);
            dropShadow.width = spritewidth;
            dropShadow.height = spriteHeight;
            AddChild(dropShadow);
        }

        public void Update()
        {
            //If the projectile is fired
            if (!HasLeftSource)
            {
                CheckIfLeftSource();
            }

            if (InWall)
            {
                CheckIfLeftWall();
            }

            if (isExploding)
            {
                WhileExploding();
            }
            else
            {
                if (shouldHome)
                {
                    RotateTowardsObject(target);
                }

                Move(moveSpeed, 0);
            }

            projectileAnimation.Animate();
        }

        /// <summary>
        /// Called when the projectile is reflected
        /// </summary>
        public void Reflect()
        {
            shouldHome = false;
            BecomeLethal();
        }

        /// <summary>
        /// Rotate towards the designated direction
        /// </summary>
        /// <param name="targetRotation">The direction the projectile should rotate towards</param>
        public void RotateTowardsDirection(Vec2 targetRotation)
        {
            if (!isExploding)
            {
                MoveDirection = targetRotation;
                MoveDirection.Normalize();

                rotation = targetRotation.GetAngleDegrees(); //Set the rotation
                if (dropShadow != null)
                {
                    dropShadow.rotation = -rotation;
                }
            }
        }

        /// <summary>
        /// Rotate towards a designated object
        /// </summary>
        /// <param name="target">The object to rotate towards</param>
        public void RotateTowardsObject(GameObject target)
        {
            Vector2 tempPosition = target.TransformPoint(0, 0); //Get the position of the target as a Vec2
            Vec2 targetPosition = new Vec2(tempPosition.x, tempPosition.y);
            Vec2 moveDirection = targetPosition - Position; //Calculate the direction to move in
            moveDirection.Normalize(); //Normalize the move direction

            RotateTowardsDirection(moveDirection); //Rotate towards the designated position
        }

        /// <summary>
        /// Check if the projectile has left the object it came from
        /// </summary>
        private void CheckIfLeftSource()
        {
            //If the projectile doesn't overlap with its source
            if (!HitTest(source))
                HasLeftSource = true;
        }

        /// <summary>
        /// Check if the projectile became unstuck from the wall
        /// </summary>
        private void CheckIfLeftWall()
        {
            foreach(Wall wall in game.FindObjectsOfType<Wall>())
            {
                if (HitTest(wall))
                {
                    return;
                }
            }

            InWall = false;
        }

        /// <summary>
        /// Spawns the projectile in the game
        /// </summary>
        /// <param name="spawnX">The X coordinates to spawn at</param>
        /// <param name="spawnY">The Y coordinates to spawn at</param>
        public void Spawn(float spawnX, float spawnY)
        {
            foreach(Wall wall in game.FindObjectsOfType<Wall>())
            {
                if (HitTest(wall))
                {
                    InWall = true;
                }
            }

            x = spawnX;
            y = spawnY;
            game.AddChild(this);
            OnShot?.Invoke(projectileType);
        }

        /// <summary>
        /// Duplicate the projectile
        /// </summary>
        /// <param name="newSource">The object the projectile came from</param>
        /// <param name="newTarget">The target the projectile will focus on if homing</param>
        /// <returns></returns>
        public abstract Projectile Duplicate(GameObject newSource, GameObject newTarget);

        /// <summary>
        /// Runs while the projectile is exploding
        /// </summary>
        private void WhileExploding()
        {
            if (Time.now >= explosionStartTime + explosionDuration)
            {
                Dissapear();
            }
        }

        /// <summary>
        /// Makes the projectile lethal to its source object
        /// </summary>
        public void BecomeLethal()
        {
            IsLethal = true;
        }

        /// <summary>
        /// Starts exploding the projectile
        /// </summary>
        public void StartExploding()
        {
            if (!isExploding)
            {
                isExploding = true;
                projectileAnimation.SetCycle(explosionAnimationFrame);
                explosionStartTime = Time.now;
                OnExplode?.Invoke(projectileType);
            }
        }

        /// <summary>
        /// Removes the projectile
        /// </summary>
        private void Dissapear()
        {
            LateDestroy();
        }

        public void OnDestroy()
        {
            GameManager.OnPlayerDeath -= Dissapear;
        }
    }
}
