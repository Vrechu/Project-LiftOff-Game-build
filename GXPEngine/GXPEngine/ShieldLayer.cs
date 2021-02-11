using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

class ShieldLayer : Sprite
{
    private Player _player;
    private Vec2 shieldDirectionVector;

    public ShieldLayer(string fileName, Player player, float Offset) : base(fileName)
    {
        SetOrigin(0, height / 2);
        _player = player;
        y = Offset;
    }

    void Update()
    {
        UpdateShieldDirectionVector();
        UpdateRotation();
        
    }

    // calculates the vector from player tou mouse position
    void UpdateShieldDirectionVector()
    {
        shieldDirectionVector.SetXY(Input.mouseX - _player.Position.x, Input.mouseY - _player.Position.y);
    }

    // sets the rotation of the shield to face the mouse position
    void UpdateRotation()
    {
        rotation = shieldDirectionVector.GetAngleDegrees();
    }
}

