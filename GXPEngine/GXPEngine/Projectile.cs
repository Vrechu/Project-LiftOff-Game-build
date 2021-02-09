namespace GXPEngine
{
    class Projectile : Sprite
    {
        private float moveSpeed = 5f; //The speed at which the projectile moves
        private float distanceFromSource = 20f; //The distance from its source
        public bool IsFired { get; private set; } = false; //Whether the projectile has been fired
        public bool HasLeftSource { get; private set; } = false; //Whether the projectile has left its source
        private GameObject source; //The source object that the projectile came from

        private int chargeStartTime; //The time the projectile started charging
        private int chargeDuration; //The time it takes for the projectile to charge

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
        /// <param name="newChargeDuration">The time it will take to charge this projectile</param>
        public Projectile(GameObject newSource, int newChargeDuration) : base("circle.png")
        {
            Initialize(newSource, newChargeDuration);
            StartCharging();
        }

        private void Initialize(GameObject newSource, int newChargeDuration)
        {
            SetOrigin(width / 2, height + distanceFromSource); //Set the origin
            source = newSource; //Set the source
            scale = 0.5f; //Set the scale
            SetXY(source.x, source.y); //Set the position
            game.AddChild(this); //Add the projectile to the game
            chargeDuration = newChargeDuration; //Set the charge duration

            name = "Projectile";
        }

        void Update()
        {
            //If the projectile is fired
            if (IsFired)
            {
                if (!HasLeftSource)
                    CheckIfLeftSource();

                Move(0, -moveSpeed); //Move in the fired direction
            }
            else
            {
                Charge(); //Charge the shot
                SetXY(source.x, source.y); //Otherwise follow the position of the source
            }
        }

        /// <summary>
        /// Start charging the projectile
        /// </summary>
        private void StartCharging()
        {
            chargeStartTime = Time.now; //Set the time the projectile started charging
        }

        /// <summary>
        /// Charge the projectile
        /// </summary>
        private void Charge()
        {
            //If enough time has passed
            if (Time.now >= chargeStartTime + chargeDuration)
                Shoot(); //Shoot the projectile
        }

        /// <summary>
        /// Rotates the projectile towards a certain position
        /// </summary>
        /// <param name="targetPosition">The position to rotate towards</param>
        public void RotateTowardsDirection(Vec2 targetPosition)
        {
            Vec2 targetRotation = targetPosition - position; //Get the rotation to face the target position
            rotation = targetRotation.GetAngleDegrees() + 90; //Set the rotation
        }

        /// <summary>
        /// Rotates the projectile towards a scertain object
        /// </summary>
        /// <param name="target">The object to rotate towards</param>
        public void RotateTowardsObject(Transformable target)
        {
            Vec2 targetPosition = new Vec2(target.x, target.y); //Get the position of the target
            RotateTowardsDirection(targetPosition); //Rotate towards the designated position
        }

        /// <summary>
        /// Shoot the projectile
        /// </summary>
        /// <param name="launchDirection">The direction to shoot the projectile in</param>
        public void Shoot()
        {
            IsFired = true; //Set state to having been fired
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
    }
}
