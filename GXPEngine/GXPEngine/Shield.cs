using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

class Shield : Sprite
{
    Player _player;
    Vec2 shieldPosition;
    Vec2 mouseVector;
    public Shield(Player player) : base("triangle.png")
    {        
        SetOrigin(this.width / 2, 0);
        _player = player;
    }

    void Update()
    {
        UpdateMouseVector();
        UpdateRotation();
    }
    void UpdateMouseVector()
    {
        mouseVector.SetXY(Input.mouseX - _player.Position.x, Input.mouseY - _player.Position.y);
    }

    void UpdateRotation()
    {
        rotation = mouseVector.GetAngleDegrees() - 90;
    }


}

