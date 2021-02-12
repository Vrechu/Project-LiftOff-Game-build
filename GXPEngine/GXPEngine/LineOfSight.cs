using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    class LineOfSight : Sprite
    {
        public bool CollidingWithEnemy { get; private set; } = true; //Whether the line of sight is colliding with another enemy
        private Transformable source; //The source of the line of sight

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="newSource">The source object of the line of sight</param>
        public LineOfSight(Transformable newSource) : base("square.png")
        {
            Initialize(newSource);
        }

        private void Initialize(Transformable newSource)
        {
            SetOrigin(width / 2, height);
            alpha = 0;

            source = newSource;
        }

        public void Update()
        {
            CollidingWithEnemy = false;
        }

        /// <summary>
        /// Set the rotation of the line of sight
        /// </summary>
        /// <param name="targetRotation">The rotation to set it at</param>
        public void RotateTowards(Vec2 targetRotation)
        {
            rotation = targetRotation.GetAngleDegrees() + 90;
        }

        /// <summary>
        /// Set the length of the line of sight
        /// </summary>
        /// <param name="newLength">The new length of the line of sight</param>
        public void SetLength(float newLength)
        {
            height = (int)newLength;
        }

        public void OnCollision(GameObject other)
        {
            if(other is Enemy && other != source && other.parent != source)
            {
                CollidingWithEnemy = true;
            }
        }
    }
}
