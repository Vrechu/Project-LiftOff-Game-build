using System;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Enemies;

class EnemySpawnPoint : Sprite
{
    //========== EDITABLE ==========
    #region EDITABLE VARIABLES
        private int _spawnTimer = 10000; // countdown time in milliseconds 
        public int EnemiesToSpawn { get; private set; } = 1; //The amount of enemies to spawn per _spawnTimer per spawn point
        private int minWaitTime = 500; //The minimum time before enemies start spawning
        private int maxWaitTime = 5000; //The maximum time before enemies start spawning

        private int maxEnemiesToSpawn = 4; //The maximum amount of enemies to spawn per _spawnTimer per spawn point
    #endregion

    private Player _player; // target of enemies 

    private int _lastSpawnTime; // last time enemy spawned 

    public static event Action OnEnemySpawned; //When an enemy is spawned

    private bool canSpawnEnemies = true; //Whether more enemies can spawn

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="px">The X coordinate to spawn at</param>
    /// <param name="py">The Y coordinate the spawn at</param>
    /// <param name="player">Reference to the player</param>
    public EnemySpawnPoint(float px, float py, Player player) : base("circle.png")
    {
        _lastSpawnTime = (Time.now - _spawnTimer) + Utils.Random(minWaitTime, maxWaitTime);
        SetOrigin(width / 2, height / 2);
        x = px;
        y = py;
        _player = player;
        game.AddChild(this);
        alpha = 0;
    }

    void Update()
    {
        SpawnTimer();
    }

    /// <summary>
    /// Spawns a new enemy
    /// </summary>
    public void SpawnEnemy()
    {
        if (Arena.CanSpawnMoreEnemies)
        {
            switch (Arena.RandomEnemy)
            {
                case 0:
                    new SlowEnemy(x, y, _player);
                    break;
                case 1:
                    new FastEnemy(x, y, _player);
                    break;
                case 2:
                    new HomingEnemy(x, y, _player);
                    break;
                default:
                    return;
            }

            OnEnemySpawned.Invoke();
        }
    }

    /// <summary>
    /// Checks the timer for spawning enemies
    /// </summary>
    public void SpawnTimer()
    {
        if (Time.now > _lastSpawnTime + (_spawnTimer / EnemiesToSpawn))
        {
            _lastSpawnTime = Time.now;
            SpawnEnemy();
        }
    }

    /// <summary>
    /// Increases the number of enemies to spawn per _spawnTimer
    /// </summary>
    /// <param name="increase">The amount of enemies to increase by</param>
    public void IncreaseEnemies(int increase)
    {
        if(EnemiesToSpawn < maxEnemiesToSpawn)
        {
            EnemiesToSpawn += increase;
        }

        Console.WriteLine(EnemiesToSpawn);
    }
}
