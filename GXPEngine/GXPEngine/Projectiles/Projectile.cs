using GXPEngine.Core;
using GXPEngine.Projectiles;
using System;
using System.Drawing;
using System.Security.Policy;

namespace GXPEngine
{
    abstract class Projectile : Sprite
    {
        //========== OVERRIDEABLE ==========
        #region
        protected float moveSpeed = 5f; //The speed at which the projectile moves

        protected int shootAnimationStartFrame = 0;
        protected int shootAnimationFrameCount = 2;

        protected int hitboxXOffset = 0;
        protected int hitboxYOffset = 0;

        protected int explosionAnimationFrame = 2;
        private int explosionDuration = 200;
        protected bool shouldHome = false;
        #endregion

        //============= EVENTS =============
        #region
        public static event Action<ProjectileType> OnShot; //When the projectile is shot
        public static event Action<ProjectileType> OnExplode; //When the projectile explodes
        #endregion

        private bool isExploding = false;
        private int explosionStartTime;

        public ProjectileType projectileType = ProjectileType.SLOW;

        public bool HasLeftSource { get; private set; } = false; //Whether the projectile has left its source
        protected GameObject source; //The source object that the projectile came from
        protected GameObject target;
        public bool IsLethal { get; private set; } = false;

        private AnimationSprite projectileAnimation;
        private Sprite dropShadow;

        private Vec2 moveDirection;

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
        /// Constrtuctor
        /// </summary>
        /// <param name="spawnX">The X coordinate to spawn the projectile at</param>
        /// <param name="spawnY">The Y coordinate to spawn the projectile at</param>
        /// <param name="newSource">The source the projectile was shot from</param>
        /// <param name="direction">The direction the projectile is being shot at</param>
        public Projectile(string hitboxSprite) : base(hitboxSprite)
        {
            Initialize();
        }

        private void Initialize()
        {
            color = 0x00ff06;
            alpha = 0;

            name = "Projectile";
        }

        protected void SetHitbox()
        {
            SetOrigin(width / 2, height / 2); //Set the origin
        }

        protected void SetAnimation(string animationSprite, int spriteCols, int spriteRows, int spriteFrames)
        {
            projectileAnimation = new AnimationSprite(animationSprite, spriteCols, spriteRows, frames: spriteFrames);
            projectileAnimation.SetOrigin(width / 2 + hitboxXOffset, height / 2 + hitboxYOffset);

            projectileAnimation.SetCycle(shootAnimationStartFrame, shootAnimationFrameCount);

            AddChild(projectileAnimation);
        }

        protected void SetShadow(string shadowSprite, int spritewidth, int spriteHeight)
        {
            dropShadow = new Sprite(shadowSprite);
            dropShadow.SetOrigin(dropShadow.width / 2, -dropShadow.height + 22);
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

                Collision col = MoveUntilCollision(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed, dropShadow, game.FindObjectsOfType<Wall>()); //Move in the fired direction

                if (col != null)
                {
                    StartExploding();
                }
            }

            projectileAnimation.Animate();
        }

        public void Reflect()
        {
            shouldHome = false;
            BecomeLethal();
        }

        /// <summary>
        /// Rotates the projectile towards a certain position
        /// </summary>
        /// <param name="targetRotation">The position to rotate towards</param>
        public void RotateTowardsDirection(Vec2 targetRotation)
        {
            if (!isExploding)
            {
                moveDirection = targetRotation;
                moveDirection.Normalize();

                rotation = targetRotation.GetAngleDegrees(); //Set the rotation
                if (dropShadow != null)
                {
                    dropShadow.rotation = -rotation;
                }
            }
        }

        /// <summary>
        /// Rotates the projectile towards a scertain object
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
        /// Check if the projectile has left its source
        /// </summary>
        private void CheckIfLeftSource()
        {
            //If the projectile doesn't overlap with its source
            if (!HitTest(source))
                HasLeftSource = true;
        }

        public void Spawn(float spawnX, float spawnY)
        {
            x = spawnX;
            y = spawnY;
            game.AddChild(this);
            OnShot?.Invoke(projectileType);
        }

        /// <summary>
        /// Duplicates the current projectile
        /// </summary>
        /// <param name="newSource">The source of the projectile</param>
        /// <returns></returns>
        public abstract Projectile Duplicate(GameObject newSource, GameObject newTarget);

        private void WhileExploding()
        {
            if (Time.now >= explosionStartTime + explosionDuration)
            {
                LateDestroy();
            }
        }

        public void BecomeLethal()
        {
            IsLethal = true;
        }

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
    }
}
