using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using GXPEngine;

class Arena : Sprite
{
    private MyGame _myGame; //reference to myGame
    private Player _player; //reference to the player

    private Sprite _arenaWalls; //The arena walls sprite
    private AnimationSprite _leftCrowd; //The left crowd sprite
    private AnimationSprite _rightCrowd; //The right crowd sprite
    private AnimationSprite _duck; //The duck sprite
    private AnimationSprite _guards; //The guards sprite
    private byte animationLength = 10; //The number of frames to show each animation frame
    private bool canInvert = true; //Whether the duck sprite can be inverted
    private bool isInverted = false; //Whether the duck sprite is inverted

    private Sprite _scoreBoard; //The scoreboard sprite
    private Sprite _highScoreBoard; //The highscoreboard sprite

    // The odds for each enemy to spawn
    public static int[,] EnemySpawnChance { get; } = new int[,]
    {
        { 400, 100,  0 }, //The odds of the enemy spawning
        {   0,  10, 15 }  //The amount the odds should increase by
    };

    //The default odds of enemies spawning
    private int[] defaultSpawnChance = new int[]
    {
        300, 100, 0
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
    } //Returns a random number corresponding to an enemy
    public static int NumberOfEnemies; //The number of enemies alive in the arena

    private EnemySpawnPoint[] enemySpawnPoints; //The enemy spawn points
    private Sprite mouseCursor; //The mouse cursor


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="myGame">Reference to MyGame</param>
    public Arena(MyGame myGame) : base("Arenav3.png")
    {
        SetEvents();

        _myGame = myGame;
        CanSpawnMoreEnemies = true;

        NumberOfEnemies = 0;

        SetCursor();

        AddChild(new Sprite("top_wall.png"));

        AddChild(_player = new Player(width / 2, height / 2));

        SetEnemySpawning();

        SetArenaWalls();

        SetAudience();

        SetHud();
    }

    /// <summary>
    /// Sets the events
    /// </summary>
    private void SetEvents()
    {
        EnemySpawnPoint.OnEnemySpawned += IncreaseNumberOfEnemiesAlive;
        Enemy.OnEnemyDestroyed += DecreaseNumberOfEnemiesAlive;
        GameManager.OnPlayerDeath += StopSpawning;
    }

    /// <summary>
    /// Sets the mouse cursor
    /// </summary>
    private void SetCursor()
    {
        mouseCursor = new Sprite("SummoningCircle.png");
        mouseCursor.SetOrigin(mouseCursor.width / 2, mouseCursor.height / 2);
        mouseCursor.width = 119;
        mouseCursor.height = 50;
        AddChild(mouseCursor);
    }

    /// <summary>
    /// Sets the properties related to spawning enemies
    /// </summary>
    private void SetEnemySpawning()
    {
        for (int i = 0; i < EnemySpawnChance.Length / 2; i++)
        {
            EnemySpawnChance[0, i] = defaultSpawnChance[i];
        }

        enemySpawnPoints = new EnemySpawnPoint[]
        {
            new EnemySpawnPoint(120,280, _player),
            new EnemySpawnPoint(width - 120, 280, _player),
            new EnemySpawnPoint(120, height - 100, _player),
            new EnemySpawnPoint(width - 120, height - 100, _player)
        };

        lastDifficultyIncrease = Time.now;
        lastEnemyIncrease = Time.now;
    }

    /// <summary>
    /// Sets the arena walls
    /// </summary>
    private void SetArenaWalls()
    {
        AddChild(_arenaWalls = new Sprite("ArenaWalls.png"));

        AddChild(new Wall(0, 0, width, 330));
        AddChild(new Wall(0, height, width, 30));
        AddChild(new Wall(-5, 0, 10, height));
        AddChild(new Wall(width - 5, 0, 10, height));
    }

    /// <summary>
    /// Sets the audience
    /// </summary>
    private void SetAudience()
    {
        AddChild(_leftCrowd = new AnimationSprite("left_rows.png", 3, 1, 3));
        AddChild(_rightCrowd = new AnimationSprite("right_rows.png", 3, 1, 3));
        AddChild(_duck = new AnimationSprite("duck.png", 4, 2, 8));
        AddChild(_guards = new AnimationSprite("guards.png", 2, 1, 2));

        _leftCrowd.SetCycle(0, 3, animationLength);
        _rightCrowd.SetCycle(0, 3, animationLength);
        _duck.SetCycle(0, 8, animationLength);
        _guards.SetCycle(0, 2, 20);
    }

    /// <summary>
    /// Sets the HUD
    /// </summary>
    private void SetHud()
    {
        AddChild(_scoreBoard = new Sprite("ScoreBoard.png"));
        _scoreBoard.SetScaleXY(0.5f, 0.5f);
        _scoreBoard.SetXY(0, -50);
        AddChild(_highScoreBoard = new Sprite("HighScoreBoard.png"));
        _highScoreBoard.SetScaleXY(0.5f, 0.5f);
        _highScoreBoard.SetXY(_scoreBoard.width, -50);

        AddChild(new HUD(game.width, game.height, _myGame));
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

        FollowCursor();
        AnimateArena();
    }

    /// <summary>
    /// Increases the number of enemies that are alive
    /// </summary>
    private void IncreaseNumberOfEnemiesAlive()
    {
        NumberOfEnemies++;
        if (NumberOfEnemies >= maxEnemiesAlive)
        {
            StopSpawning();
        }
    }
    
    /// <summary>
    /// Decreases the number of enemies that are alive
    /// </summary>
    private void DecreaseNumberOfEnemiesAlive()
    {
        NumberOfEnemies--;

        if (NumberOfEnemies < maxEnemiesAlive)
        {
            CanSpawnMoreEnemies = true;
        }
    }

    /// <summary>
    /// Updates the difficulty of the game
    /// </summary>
    private void UpdateDifficulty()
    {
        Console.WriteLine("Updated Difficulty");

        for (int i = 0; i < EnemySpawnChance.Length / 2; i++)
        {
            EnemySpawnChance[0, i] += EnemySpawnChance[1, i];
        }

        lastDifficultyIncrease = Time.now;
    }

    /// <summary>
    /// Increases the number of enemies to spawn
    /// </summary>
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

    /// <summary>
    /// Makes the cursor sprite follow the cursor
    /// </summary>
    private void FollowCursor()
    {
        mouseCursor.SetXY(Input.mouseX, Input.mouseY);
    }

    protected override void OnDestroy()
    {
        EnemySpawnPoint.OnEnemySpawned -= IncreaseNumberOfEnemiesAlive;
        Enemy.OnEnemyDestroyed -= DecreaseNumberOfEnemiesAlive;
        GameManager.OnPlayerDeath -= StopSpawning;
    }

    /// <summary>
    /// Animate the arena
    /// </summary>
    private void AnimateArena()
    {
        _leftCrowd.Animate();
        _rightCrowd.Animate();
        _duck.Animate();
        _guards.Animate();

        if (_duck.currentFrame == 7)
        {
            canInvert = true;
        }

        if (_duck.currentFrame == 0 && canInvert == true)
        {
            if (!isInverted)
            {
                isInverted = true;
                _duck.Mirror(true, false);
            }
            else if (isInverted)
            {isInverted = false;
                _duck.Mirror(false, false);
            }
            canInvert = false;
        }
    }

    /// <summary>
    /// Stop spawning enemies
    /// </summary>
    private void StopSpawning()
    {
        CanSpawnMoreEnemies = false;
    }
}
