using System;
using System.Collections.Generic;
using GXPEngine.Projectiles;
using System.Linq;
using System.Text;
using GXPEngine;

class AudioPlayer : GameObject
{
    MyGame _myGame;
       public AudioPlayer(MyGame myGame)
    {
        _myGame = myGame;
        EventSubscriptions();
    }      
    
    private void EventSubscriptions()
    {
        Player.OnPLayerHit += PlayHurtSound;
        Projectile.OnShot += PlayProjectileShotSound;
    }

    void OnDestroy()
    {
        Player.OnPLayerHit -= PlayHurtSound;
        Projectile.OnShot -= PlayProjectileShotSound;
    }

    private void PlayHurtSound()
    {
        new Sound("character_hit.mp3").Play(); // ...play a sound
    }

    private void PlayDeathSound()
    {

    }

    private void PlayReflectSound()
    {
        
    }

    private void PlayProjectileShotSound(ProjectileType projectileType)
    {

    }
}

