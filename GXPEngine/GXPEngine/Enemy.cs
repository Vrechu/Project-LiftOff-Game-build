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

        public Enemy(string enemySprite) : base(enemySprite)
        {
            Initialize();
        }

        private void Initialize()
        {
            SetOrigin(width / 2, height / 2); //Center the origin of the enemy
        }

        public void Update()
        {
            RotateTowards();
            MoveTowards();
        }

        /// <summary>
        /// Move towards the target
        /// </summary>
        private void MoveTowards(/*Transformable target*/)
        {
            Vec2 target = new Vec2(Input.mouseX, Input.mouseY); //TODO: Placeholder target

            float distanceToTarget = new Vec2(target.x - position.x, target.y - position.y).Length();

            if (distanceToTarget > moveUntilDistance)
            {
                Vec2 targetPosition = target - position;
                targetPosition.Normalize();

                position += targetPosition * moveSpeed;
            }
        }

        /// <summary>
        /// Rotate towards the target
        /// </summary>
        private void RotateTowards(/*Transformable target*/)
        {
            Vec2 target = new Vec2(Input.mouseX, Input.mouseY); //TODO: Placeholder target

            target = target - position;

            rotation = target.GetAngleDegrees() + 90;
        }
    }
}
