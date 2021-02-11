using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

class GameManager
{
    private static GameManager singleton;
    public static MyGame _myGame;
    public static GameManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = new GameManager(_myGame);
            }
            return singleton;
        }
    }

    private int _maxPlayerHealth = 3;
    public int _playerHealth;

    private GameManager(MyGame myGame)
    {
        _myGame = myGame;
        _playerHealth = _maxPlayerHealth;
    }

    //removes the specified amount of health from playerhealth
    public void PlayerGetsHit(int damage)
    {      
        _playerHealth -= damage;
        _playerHealth = Mathf.Max(0, _playerHealth);
        Console.WriteLine("OUCH! " + _playerHealth);
    }

    //resets the player health to max health
    public void ResetHealth()
    {
        _playerHealth = _maxPlayerHealth;
    }

    

    
}

