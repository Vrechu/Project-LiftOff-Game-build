using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

class EnemySpawnPoint : Sprite
{
    private Player _player; // target of enemies

    private int _spawnTimer; // countdown timer for spawnting enemies
    private int _spawnCounterTime = 500; // countdown time

    public EnemySpawnPoint(float px, float py, Player player) : base("circle.png")
    {
        SetOrigin(width / 2, height / 2);
        x = px;
        y = py;
        _player = player;
        _spawnTimer = _spawnCounterTime;
        game.AddChild(this);
    }

    void Update()
    {
        SpawnTimer();
    }
    
    // spawns an enemy
    public void SpawnEnemy()
    {
        new Enemy(x,y, _player);
    }

    //timer for spawning enemies
    public void SpawnTimer()
    {
        _spawnTimer--;
        if (_spawnTimer == 0)
        {
            _spawnTimer = _spawnCounterTime;
            SpawnEnemy();
        }
        
    }
}

