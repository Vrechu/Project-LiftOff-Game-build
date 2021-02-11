using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

class EnemySpawnPoint : Sprite
{
    private Player _player; // target of enemies

    private int _lastSpawnTime; // last time enemy spawned
    private int _spawnTimer = 5; // countdown time in seconds

    public EnemySpawnPoint(float px, float py, Player player) : base("circle.png")
    {
        SetOrigin(width / 2, height / 2);
        x = px;
        y = py;
        _player = player;
        game.AddChild(this);
    }

    void Update()
    {
        SpawnTimer();
    }
    
    // spawns an enemy
    public void SpawnEnemy()
    {        
        new SlowEnemy (x,y, _player);
    }

    //timer for spawning enemies
    public void SpawnTimer()
    {
        if (Time.now > _lastSpawnTime + _spawnTimer * 1000)
        {
            _lastSpawnTime = Time.now;
            SpawnEnemy();
        }
        
    }
}

