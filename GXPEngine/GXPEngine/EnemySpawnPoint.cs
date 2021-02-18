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
        private int minWaitTime = 500;
        private int maxWaitTime = 5000;
    #endregion

    private Player _player; // target of enemies 

    private int _lastSpawnTime; // last time enemy spawned 
    private int maxEnemiesToSpawn = 4; //The maximum amount of enemies to spawn per _spawnTimer per spawn point

    public static event Action OnEnemySpawned;

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

    // spawns an enemy 
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

    //timer for spawning enemies 
    public void SpawnTimer()
    {
        if (Time.now > _lastSpawnTime + (_spawnTimer / EnemiesToSpawn))
        {
            _lastSpawnTime = Time.now;
            SpawnEnemy();
        }
    }

    public void IncreaseEnemies(int increase)
    {
        if(EnemiesToSpawn < maxEnemiesToSpawn)
        {
            EnemiesToSpawn += increase;
        }

        Console.WriteLine(EnemiesToSpawn);
    }
}
