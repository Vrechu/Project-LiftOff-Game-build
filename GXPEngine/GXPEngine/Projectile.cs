namespace GXPEngine
{
    class Projectile : Sprite
    {
        private float moveSpeed = 5f; //The speed at which the projectile moves
        //private float distanceFromSource = 20f; //The distance from its source
        public bool HasLeftSource { get; private set; } = false; //Whether the projectile has left its source
        private GameObject source; //The source object that the projectile came from

        //private int chargeStartTime; //The time the projectile started charging
        //private int chargeDuration; //The time it takes for the projectile to charge

        //The position in Vector2
        private Vec2 Position
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
        /// Constrtuctor
        /// </summary>
        /// <param name="spawnX">The X coordinate to spawn the projectile at</param>
        /// <param name="spawnY">The Y coordinate to spawn the projectile at</param>
        /// <param name="newSource">The source the projectile was shot from</param>
        /// <param name="direction">The direction the projectile is being shot at</param>
        public Projectile(float spawnX, float spawnY, GameObject newSource, Vec2 direction) : base("circle.png")
        {
            Initialize(spawnX, spawnY, newSource);
            RotateTowardsDirection(direction);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="spawnX">The X coordinate to spawn the projectile at</param>
        /// <param name="spawnY">The Y coordinate to spawn the projectile at</param>
        /// <param name="newSource">The source the projectile was shot from</param>
        /// <param name="target">The target object the projectile is shot at</param>
        public Projectile(float spawnX, float spawnY, GameObject newSource, Transformable target) : base("circle.png")
        {
            Initialize(spawnX, spawnY, newSource);
            RotateTowardsObject(target);
        }

        private void Initialize(float spawnX, float spawnY, GameObject newSource)
        {
            SetOrigin(width / 2, height /2); //Set the origin
            source = newSource; //Set the source
            scale = 0.5f; //Set the scale
            SetXY(spawnX, spawnY); //Set the position
            game.AddChild(this); //Add the projectile to the game
            //chargeDuration = newChargeDuration; //Set the charge duration

            name = "Projectile";
        }

        void Update()
        {
            //If the projectile is fired
                if (!HasLeftSource)
                    CheckIfLeftSource();

                Move(0, -moveSpeed); //Move in the fired direction
            //else
            //{
            //    //Charge(); //Charge the shot
            //    SetXY(source.x, source.y); //Otherwise follow the position of the source
            //}
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
            rotation = targetRotation.GetAngleDegrees() + 90; //Set the rotation
        }

        /// <summary>
        /// Rotates the projectile towards a scertain object
        /// </summary>
        /// <param name="target">The object to rotate towards</param>
        public void RotateTowardsObject(Transformable target)
        {
            Vec2 targetPosition = new Vec2(target.x, target.y); //Get the position of the target
            Vec2 targetRotation = targetPosition - Position; //Get the rotation to rotate towards
            RotateTowardsDirection(targetRotation); //Rotate towards the designated position
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
