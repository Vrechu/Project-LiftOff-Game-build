using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine.Projectiles
{
    class ProjectileLauncher : Sprite
    {
        private Projectile projectileToLaunch; //The projectile to shoot
        private GameObject source; //The source to shoot from
        private GameObject target; //The target the projectile will focus on

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="newProjectileToLaunch">The projectile the launcher should launch copies of</param>
        /// <param name="newSource">The source object of the launcher</param>
        /// <param name="newTarget">The target of the projectiles to spawn</param>
        public ProjectileLauncher(Projectile newProjectileToLaunch, GameObject newSource, GameObject newTarget) : base("square.png", false)
        {
            Initialize(newProjectileToLaunch, newSource, newTarget);
        }

        private void Initialize(Projectile newProjectileToLaunch, GameObject newSource, GameObject newTarget)
        {
            alpha = 0;
            SetOrigin(width / 2, height / 2);
            projectileToLaunch = newProjectileToLaunch;
            source = newSource;
            target = newTarget;
        }

        /// <summary>
        /// Shoot at the designated target
        /// </summary>
        /// <param name="target">The target to shoot at</param>
        public void ShootAt(GameObject target)
        {
            Projectile projectile = projectileToLaunch.Duplicate(source, target); //Duplicate the projectile to shoot
            Vector2 worldPosition = TransformPoint(0, 0); //Get the world position of the launcher
            projectile.Spawn(worldPosition.x, worldPosition.y); //Spawn the projectile in the world
            projectile.RotateTowardsObject(target); //Shoot the projectile at the target
        }
    }
}
