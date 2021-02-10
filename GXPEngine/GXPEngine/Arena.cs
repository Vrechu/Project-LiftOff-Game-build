using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

class Arena : Sprite
{
    Player _player;
    public Arena(Game myGame) : base("arena.png")
    {
        width = myGame.width;
        height = myGame.height;
        AddChild(_player = new Player(width / 2, height / 2));
        new EnemySpawnPoint(width, height / 2, _player);
        new EnemySpawnPoint(0, height / 2, _player);
        new EnemySpawnPoint(width / 2, 0, _player);
        new EnemySpawnPoint(width / 2, height, _player);
    }
}

