using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

class Button : Sprite
{
    public Button(float px, float py) : base("square.png")
    {
        SetOrigin(width / 2, height / 2);
        SetScaleXY(0.5f, 0.5f);
        x = px;
        y = py;
    }
}

