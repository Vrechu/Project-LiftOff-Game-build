using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using GXPEngine;

class Arena : Sprite
{
    Player _player;

    public static int[,] EnemySpawnChance { get; } = new int[,]
    {
        { 400, 100,  900 }, //The actual spawn chance 
        {   0,  10, 15 }  //The amount it should increase by 
    };

    public static int RandomEnemy
    {
        get
        {
            int totalChance = 0;
            for (int i = 0; i < EnemySpawnChance.Length / 2; i++)
            {
                totalChance += EnemySpawnChance[0, i];
            }

            int random = Utils.Random(0, totalChance);
            int result = 0;

            totalChance = 0;

            for (result = 0; result < EnemySpawnChance.Length / 2; result++)
            {
                totalChance += EnemySpawnChance[0, result];

                if (totalChance >= random)
                    return result;
            }

            return 0;
        }
    }

    private EnemySpawnPoint[] enemySpawnPoints;

    private int lastDifficultyIncrease;
    private int difficultyIncreaseTimer = 5000;
    private int lastEnemyIncrease;
    private int enemyIncreaseTimer = 2000;

    public Arena(Game myGame) : base("Arenav2_2.png")
    {
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
            Console.WriteLine("Increased number of enemies to spawn");
            lastEnemyIncrease = Time.now;
        }
        catch
        {
            throw new NullReferenceException();
        }
    }
}
