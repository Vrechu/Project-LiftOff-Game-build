using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

class GameManager : GameObject
{
    private static GameManager singleton;
    public static event Action OnPlayerDeath;

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
    public int _playerHealth;

    public int _playerScore;
    public int _highScore = 0;

    private GameManager()
    {
        
        _playerHealth = _maxPlayerHealth;
    }

    void Update()
    {
        PlayerDies();
    }

    //removes the specified amount of health from playerhealth
    public void PlayerGetsHit(int damage)
    {      
        _playerHealth -= damage;
        _playerHealth = Mathf.Max(0, _playerHealth);
    }

    //resets the player health to max health
    public void ResetHealth()
    {
        _playerHealth = _maxPlayerHealth;
    }
    
    //sets player score to 0
    public void ResetScore()
    {
        _playerScore = 0;
    }

    // ends the game and starts the menu
    public void PlayerDies()
    {
        if (_playerHealth == 0)
        {
            OnPlayerDeath?.Invoke();
            CheckHighScore();
        }
    }

    private void CheckHighScore()
    {
        if (_highScore < _playerScore)
        {
            _highScore = _playerScore;
        }
    }
}