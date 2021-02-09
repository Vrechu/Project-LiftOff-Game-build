using GXPEngine.Core;
using GXPEngine.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GXPEngine
{
    class Projectile : Sprite
    {
        private float moveSpeed = 5f; //The speed at which the projectile moves
        private float distanceFromSource = 20f; //The distance from its source

        private Vec2 moveDirection; //The direction to move in
        private bool isFired = false; //Whether the projectile has been fired
        private GameObject source; //The source object that the projectile came from

        //The position in Vector2
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
        /// <param name="newSource">The source object that the projectile came from</param>
        public Projectile(GameObject newSource) : base("circle.png")
        {
            Initialize(newSource);
        }

        private void Initialize(GameObject newSource)
        {
            SetOrigin(width / 2, height + distanceFromSource); //Set the origin
            source = newSource; //Set the source
            scale = 0.5f; //Set the scale
            SetXY(source.x, source.y); //Set the
            game.AddChild(this);
        }

        void Update()
        {
            //If the projectile is fired
            if (isFired)
                position += moveDirection * moveSpeed; //Move in the fired direction
            else
            {
                SetXY(source.x, source.y); //Otherwise follow the position of the source
            }
        }

        /// <summary>
        /// Shoot the projectile
        /// </summary>
        /// <param name="launchDirection">The direction to shoot the projectile in</param>
        public void Shoot(Vec2 launchDirection)
        {
            SetOrigin(width / 2, height / 2); //Center the origin

            moveDirection = launchDirection; //Set the direction the bullet is shot at
            moveDirection.Normalize(); //Normalize the direction

            isFired = true; //Set state to having been fired
        }

        /// <summary>
        /// Rotate the projectile towards the designated target
        /// </summary>
        /// <param name="rotationTarget">The target to rotate towards</param>
        public void RotateTowards(Transformable rotationTarget)
        {
            Vec2 targetPosition = new Vec2(rotationTarget.x, rotationTarget.y); //Get the position of the target as a Vec2
            Vec2 targetRotation = targetPosition - position; //Get the rotation towards the target
            rotation = targetRotation.GetAngleDegrees() + 90; //Set the rotation of the projectile
        }
    }
}
