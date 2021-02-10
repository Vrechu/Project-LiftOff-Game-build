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

    private GameManager() { }

    private int _playerHealth;

    //
    public void PlayerGetsHit(int damage)
    {
        _playerHealth -= damage;
        _playerHealth = Mathf.Max(0, _playerHealth);
    }
}

