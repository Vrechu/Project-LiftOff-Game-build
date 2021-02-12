﻿using GXPEngine.Core;
using System.Drawing;

namespace GXPEngine
{
    abstract class Projectile : AnimationSprite
    {
        protected float moveSpeed = 5f; //The speed at which the projectile moves
        //private float distanceFromSource = 20f; //The distance from its source
        public bool HasLeftSource { get; private set; } = false; //Whether the projectile has left its source
        protected GameObject source; //The source object that the projectile came from

        private int shootAnimationStartFrame = 0;
        private int shootAnimationFrameCount = 2;

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
        public Projectile(string sprite) : base(sprite, 3, 1)
        {
            Initialize();
        }

        private void Initialize()
        {
            SetOrigin(width / 2, height / 2); //Set the origin

            SetCycle(shootAnimationStartFrame, shootAnimationFrameCount);

            name = "Projectile";
        }

        public void Update()
        {
            //If the projectile is fired
            if (!HasLeftSource)
            {
                CheckIfLeftSource();
            }

            Move(moveSpeed, 0); //Move in the fired direction
            Animate();
        }

        /// <summary>
        /// Start charging the projectile
        /// </summary>
        //private void StartCharging()
        //{
        //    chargeStartTime = Time.now; //Set the time the projectile started charging
        //}

        /// <summary>
        /// Charge the projectile
        /// </summary>
        //private void Charge()
        //{
        //    //If enough time has passed
        //    if (Time.now >= chargeStartTime + chargeDuration)
        //        Shoot(); //Shoot the projectile
        //}

        /// <summary>
        /// Rotates the projectile towards a certain position
        /// </summary>
        /// <param name="targetRotation">The position to rotate towards</param>
        public void RotateTowardsDirection(Vec2 targetRotation)
        {
            rotation = targetRotation.GetAngleDegrees(); //Set the rotation
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
        }

        /// <summary>
        /// Duplicates the current projectile
        /// </summary>
        /// <param name="newSource">The source of the projectile</param>
        /// <returns></returns>
        public abstract Projectile Duplicate(GameObject newSource);
    }
}