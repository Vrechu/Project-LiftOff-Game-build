using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

class PlayerAnimations : AnimationSprite
{
    public PlayerAnimations() : base("wizard_complete.png", 7, 3, 10)
    {
        SetOrigin(width / 2, height / 2);
    }
}

