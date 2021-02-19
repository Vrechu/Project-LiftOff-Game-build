using System;									// System contains a lot of default C# libraries 
using System.Drawing;                           // System.Drawing contains a library used for canvas drawing below
using GXPEngine;								// GXPEngine contains the engine

public class MyGame : Game
{
    private Arena _arena;
    private Menu _menu;
    private Cutscene _cutscene;

    public static event Action OnPlayerDeath;
    public static event Action OnScreenStateSwitch;
    public static event Action OnGameRun;

    public enum ScreenState
    {
        MENU, INGAME, CUTSCENE
    }
    public ScreenState _screenState = ScreenState.MENU;

    public MyGame() : base(1920, 1080, false)
    {
        loadScreens();
        AddChild(GameManager.Singleton);
        AddChild(new AudioPlayer(this));
        Player.OnDeathAnimationEnd += GameOver;
        OnGameRun?.Invoke();

    }

    void OnDestroy()
    {
        Player.OnDeathAnimationEnd -= GameOver;
    }

    void Update()
    {

    }

    static void Main()                          // Main() is the first method that's called when the program is run
    {
        new MyGame().Start();                   // Create a "MyGame" and start it
    }

    //destroys the menu then starts the game
    private void StartGame()
    {
        if (_cutscene != null)
        {
            _cutscene.LateDestroy();
            _cutscene = null;
        }

        AddChild(_arena = new Arena(this));
    }

    // destroys every enemy, projectile, and enemyspawner. destorys the arena. then opens the menu
    private void StartMenu()
    {
        if (_arena != null)
        {
            foreach (Enemy enemy in FindObjectsOfType<Enemy>())
            {
                enemy.LateDestroy();
            }
            foreach (EnemySpawnPoint spawnPoint in FindObjectsOfType<EnemySpawnPoint>())
            {
                spawnPoint.LateDestroy();
            }
            foreach (Projectile projectile in FindObjectsOfType<Projectile>())
            {
                projectile.LateDestroy();
            }
            _arena.LateDestroy();
            _arena = null;
        }
        _menu = new Menu(0, 0, this);
        LateAddChild(_menu);
    }

    //destroys the menu then starts the cutscene
    private void StartCutscene()
    {
        if (_menu != null)
        {
            _menu.LateDestroy();
            _menu = null;
        }

        _cutscene = new Cutscene(this);
        LateAddChild(_cutscene);
    }

    // switch that loads proper screen depending on the screenstate
    public void loadScreens() // determines which screens to show
    {
        switch (_screenState)
        {
            case ScreenState.MENU:
                {
                    StartMenu();
                    break;
                }
            case ScreenState.INGAME:
                {
                    StartGame();
                    break;
                }
            case ScreenState.CUTSCENE:
                {
                    StartCutscene();
                    break;
                }
        }
        OnScreenStateSwitch?.Invoke();
    }

    // ends the game and starts the menu
    public void GameOver()
    {
        _screenState = ScreenState.MENU;
        loadScreens();
        GameManager.Singleton.ResetHealth();
        GameManager.Singleton.ResetScore();
    }
}