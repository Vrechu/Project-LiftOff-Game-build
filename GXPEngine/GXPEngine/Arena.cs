using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

class Arena : Sprite
{
    Player _player;
    public Arena(Game myGame) : base("Arenav2_2.png")
    {
        width = myGame.width;
        height = myGame.height;
        AddChild(_player = new Player(width / 2, height / 2));
        new EnemySpawnPoint(120,280, _player);
        new EnemySpawnPoint(width-120, 280, _player);
        new EnemySpawnPoint(120,height, _player);
        new EnemySpawnPoint(width-120, height, _player);
        AddChild(new HUD(game.width, game.height));

        AddChild(new Wall(0, 200, width, 30));
        AddChild(new Wall(0, height - 100, width, 30));
        AddChild(new Wall(0, 0, 10, height));
        AddChild(new Wall(width - 180, 0, 10, height));
    }
}

