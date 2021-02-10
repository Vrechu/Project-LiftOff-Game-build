using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

class GameManager
{

    private static GameManager singleton;

    public static GameManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = new GameManager();
            }
            return singleton;
        }
    }

    private int _maxPlayerHealth = 3;
    private int _playerHealth;

    private GameManager()
    {
        _playerHealth = _maxPlayerHealth;
    }

    public void PlayerGetsHit(int damage)
    {
        if (_playerHealth == 0)
        {
            GameOver();
        }
            _playerHealth -= damage;        
        _playerHealth = Mathf.Max(0, _playerHealth);
        Console.WriteLine("OUCH! " + _playerHealth);
    }

    public void GameOver()
    {
        Console.WriteLine("YOU DIED");
    }
}

