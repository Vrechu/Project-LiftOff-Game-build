using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

class Arena : Sprite
{
    Player _player;
    public Arena(Game myGame) : base("Arenav2.png")
    {
        width = myGame.width;
        height = myGame.height;
        AddChild(_player = new Player(width / 2, height / 2));
        new EnemySpawnPoint(120,280, _player);
        new EnemySpawnPoint(width-120, 280, _player);
        new EnemySpawnPoint(120,height, _player);
        new EnemySpawnPoint(width-120, height, _player);
    }
}

