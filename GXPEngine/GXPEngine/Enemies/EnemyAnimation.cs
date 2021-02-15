using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine.Enemies
{
    class EnemyAnimation : AnimationSprite
    {
        public EnemyAnimation(string sprite, int cols, int rows) : base(sprite, cols, rows, addCollider:false)
        {
            Initialize();
        }

        private void Initialize()
        {
            SetOrigin(width / 2, height / 2);
        }

        public void Update()
        {
            Animate();
        }

        /// Constructor
        /// </summary>
        /// <param name="cycle">0 = standing. 1 = walking. 2 = shooting. 3 = death.</param>
        public void SetAnimationCycle(int startFrame, int animationLength, byte frameDuration)
        {
            SetCycle(startFrame, animationLength, frameDuration);
        }
    }
}
