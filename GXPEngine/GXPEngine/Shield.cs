using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

class Shield : Sprite
{
    private Player _player;
    private Vec2 shieldDirectionVector;
    private float projectileSpawnDistance = 20;
    private Vec2 projectileReflectLocation;

    public Shield(Player player) : base("Shield_Hitbox.png")
    {        
        SetOrigin(-96 , height/2);
        _player = player;
        alpha = 0;
    }

    void Update()
    {
        UpdateShieldDirectionVector();
        UpdateRotation();
        UpdateProjectileReflectLocation();
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

   
    void OnCollision(GameObject other)
    {
        if (other is Projectile)
        {
            Projectile projectile = other as Projectile;
            projectile.RotateTowardsDirection(shieldDirectionVector);
            /*projectile.SetXY(projectileReflectLocation.x, projectileReflectLocation.y);*/
        }
    }

    // calculates the point projectiles move to when reflectinmg.
    void UpdateProjectileReflectLocation()
    {
        projectileReflectLocation = _player.Position + shieldDirectionVector.Normalized() *
            (this.height + projectileSpawnDistance);
    }       
}

