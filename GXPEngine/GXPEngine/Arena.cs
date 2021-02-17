using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using GXPEngine;

class Arena : Sprite
{
    Player _player;

    /// <summary>
    /// The odds for each enemy to spawn
    /// </summary>
    public static int[,] EnemySpawnChance { get; } = new int[,]
    {
        { 400, 100,  0 }, //The odds of the enemy spawning
        {   0,  10, 15 }  //The amount the odds should increase by
    };

    private int lastDifficultyIncrease; //The last time the enemySpawnChance was updated
    private int difficultyIncreaseTimer = 10000; //The amount of time before the enemySpawnChance is updated (milliseconds)
    private int lastEnemyIncrease; //The last time more enemies started spawning
    private int enemyIncreaseTimer = 5000; //The amount of time before more enemies start spawning (milliseconds)

    private int maxEnemiesAlive = 10; //The maximum number of enemies to be alive in the arena at once
    public static bool CanSpawnMoreEnemies; //Whether the arena can spawn more enemies

    public static int RandomEnemy
    {
        get
        {
            float totalChance = 0;
            for (int i = 0; i < EnemySpawnChance.Length / 2; i++)
            {
                totalChance += EnemySpawnChance[0, i];
            }

            Console.WriteLine(Mathf.Round((100f / totalChance * EnemySpawnChance[0, 0])) + "% | " + Mathf.Round((100 / totalChance * EnemySpawnChance[0, 1])) + "% | " + Mathf.Round((100 / totalChance * EnemySpawnChance[0, 2])) + "%");

            int random = (int)Utils.Random(0, totalChance);

            totalChance = 0;

            for (int result = 0; result < EnemySpawnChance.Length / 2; result++)
            {
                totalChance += EnemySpawnChance[0, result];

                if (totalChance >= random)
                    return result;
            }

            return 0;
        }
    }
    public static int NumberOfEnemies;

    private EnemySpawnPoint[] enemySpawnPoints;

    public Arena(Game myGame) : base("Arenav2_2.png")
    {
        EnemySpawnPoint.EnemySpawned += IncreaseNumberOfEnemiesAlive;
        Enemy.EnemyDestroyed += DecreaseNumberOfEnemiesAlive;

        CanSpawnMoreEnemies = true;

        NumberOfEnemies = 0;

        width = myGame.width;
        height = myGame.height;
        AddChild(_player = new Player(width / 2, height / 2));

        enemySpawnPoints = new EnemySpawnPoint[]
        {
            new EnemySpawnPoint(120,280, _player),
            new EnemySpawnPoint(width - 120, 280, _player),
            new EnemySpawnPoint(120, height, _player),
            new EnemySpawnPoint(width - 120, height, _player)
        };

        AddChild(new HUD(game.width, game.height));

        AddChild(new Wall(0, 200, width, 30));
        AddChild(new Wall(0, height - 100, width, 30));
        AddChild(new Wall(0, 0, 10, height));
        AddChild(new Wall(width - 180, 0, 10, height));

        lastDifficultyIncrease = Time.now;
        lastEnemyIncrease = Time.now;
    }

    public void Update()
    {
        if (Time.now >= lastDifficultyIncrease + difficultyIncreaseTimer)
        {
            UpdateDifficulty();
        }

        if (Time.now >= lastEnemyIncrease + enemyIncreaseTimer)
        {
            IncreaseEnemyCount();
        }
    }

    private void IncreaseNumberOfEnemiesAlive()
    {
        NumberOfEnemies++;
        if(NumberOfEnemies >= maxEnemiesAlive)
        {
            CanSpawnMoreEnemies = false;
        }
    }

    private void DecreaseNumberOfEnemiesAlive()
    {
        NumberOfEnemies--;

        if (NumberOfEnemies < maxEnemiesAlive)
        {
            CanSpawnMoreEnemies = true;
        }
    }

    private void UpdateDifficulty()
    {
        Console.WriteLine("Updated Difficulty");

        for (int i = 0; i < EnemySpawnChance.Length / 2; i++)
        {
            EnemySpawnChance[0, i] += EnemySpawnChance[1, i];
        }

        lastDifficultyIncrease = Time.now;
    }

    private void IncreaseEnemyCount()
    {
        try
        {
            int minEnemyNumber = enemySpawnPoints.Min(r => r.EnemiesToSpawn);
            enemySpawnPoints.Where(r => r.EnemiesToSpawn == minEnemyNumber).First().IncreaseEnemies(1);
            //Console.WriteLine("Increased number of enemies to spawn");
            lastEnemyIncrease = Time.now;
        }
        catch
        {
            throw new NullReferenceException();
        }
    }

    protected override void OnDestroy()
    {
        EnemySpawnPoint.EnemySpawned -= IncreaseNumberOfEnemiesAlive;
        Enemy.EnemyDestroyed -= DecreaseNumberOfEnemiesAlive;
    }
}
