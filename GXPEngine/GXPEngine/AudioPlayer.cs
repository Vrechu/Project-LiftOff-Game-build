using System;
using System.Collections.Generic;
using GXPEngine.Projectiles;
using GXPEngine.Enemies;
using System.Linq;
using System.Text;
using GXPEngine;

class AudioPlayer : GameObject
{
    MyGame _myGame;
    SoundChannel _musicChannel;
    Sound _gameMusic;           //music to play ingame
    Sound _menuMusic;           //music to play in menu
    Sound _comicMusic;          // music to play in the comic

    public AudioPlayer(MyGame myGame)
    {
        _myGame = myGame;
        EventSubscriptions();
        _gameMusic = new Sound("battledrums.mp3", true ,true);
        _menuMusic = new Sound("main_menu.wav", true, true);
        _comicMusic = new Sound("comic_music.mp3", true, true);
    }

    // subscribe to sound events
    private void EventSubscriptions()
    {
        MyGame.OnGameRun += PlayMusic;
        MyGame.OnScreenStateSwitch += PlayMusic;
        Player.OnPLayerHit += PlayHurtSound;
        GameManager.OnPlayerDeath += PlayDeathSound;
        Projectile.OnShot += PlayProjectileShotSound;
        Shield.OnProjectileReflect += PlayReflectSound;
        Enemy.OnHit += PlayEnemyHitSound;
    }

    // unsubscribe from sound events
    void OnDestroy()
    {
        MyGame.OnGameRun -= PlayMusic;
        MyGame.OnScreenStateSwitch -= PlayMusic;
        Player.OnPLayerHit -= PlayHurtSound;
        GameManager.OnPlayerDeath -= PlayDeathSound;
        Projectile.OnShot -= PlayProjectileShotSound;
        Shield.OnProjectileReflect -= PlayReflectSound;
        Enemy.OnHit -= PlayEnemyHitSound;
    }

    // plays the sound effect for when the player is hurt
    private void PlayHurtSound()
    {
        new Sound("character_hit.wav").Play();
    }

    // plays the sound effect for dying
    private void PlayDeathSound()
    {
        _musicChannel.Stop();                   //stops the music channel
        new Sound("PlayerDead.wav").Play();
    }

    // plays the sound effect for reflecting
    private void PlayReflectSound()
    {
        new Sound("reflect_sound.wav").Play();
    }

    //plays sound effect depending on the projectile type
    private void PlayProjectileShotSound(ProjectileType projectileType)
    {
        switch (projectileType)
        {
            case ProjectileType.SLOW:
                {
                    new Sound("fireball.wav").Play();
                    break;
                }
            case ProjectileType.FAST:
                {
                    new Sound("laser.wav").Play();
                    break;
                }
            case ProjectileType.HOMING:
                {
                    new Sound("bone_throw1.wav").Play();
                    break;
                }
        }
    }

    // playes a sound effect depending enemy type
    private void PlayEnemyHitSound(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.SLOW:
                {
                    new Sound("enemy_lizard.wav").Play();
                    break;
                }
            case EnemyType.FAST:
                {
                    new Sound("enemy_lizard.wav").Play();
                    break;
                }
            case EnemyType.HOMING:
                {
                    new Sound("enemy_bone.wav").Play();
                    break;
                }
        }
    }

    // plays music depending on the screen state
    void PlayMusic()
    {
        if (_musicChannel != null)
        {
            _musicChannel.Stop();
        }
        switch (_myGame._screenState)
        {
            case MyGame.ScreenState.INGAME:
                {                    
                    _musicChannel = _gameMusic.Play();
                    break;
                }
            case MyGame.ScreenState.MENU:
                {
                    _musicChannel = _menuMusic.Play();
                    break;
                }
            case MyGame.ScreenState.CUTSCENE:
                {
                    _musicChannel = _comicMusic.Play();
                    break;
                }
        }
    }
}

