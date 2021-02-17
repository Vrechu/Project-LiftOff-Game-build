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
    Sound _gameMusic;
    Sound _menuMusic;
    Sound _comicMusic;
    SoundChannel _effectsChannel;
    Sound _effect;

    public AudioPlayer(MyGame myGame)
    {
        _myGame = myGame;
        EventSubscriptions();
        _gameMusic = new Sound("test_music.mp3", true ,true);
        _menuMusic = new Sound("test_music.mp3", true, true);
        _comicMusic = new Sound("test_music.mp3", true, true);
    }

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

    void OnDestroy()
    {
        MyGame.OnGameRun += PlayMusic;
        MyGame.OnScreenStateSwitch -= PlayMusic;
        Player.OnPLayerHit -= PlayHurtSound;
        GameManager.OnPlayerDeath -= PlayDeathSound;
        Projectile.OnShot -= PlayProjectileShotSound;
        Shield.OnProjectileReflect -= PlayReflectSound;
        Enemy.OnHit += PlayEnemyHitSound;
    }

    private void PlayHurtSound()
    {
        new Sound("character_hit.wav").Play();
    }

    private void PlayDeathSound()
    {
        _musicChannel.Stop();
        new Sound("PlayerDead.wav").Play();
    }

    private void PlayReflectSound()
    {
        new Sound("reflect_sound.wav").Play();
    }

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
                    new Sound("ping.wav").Play();
                    break;
                }
            case ProjectileType.HOMING:
                {
                    new Sound("bone_throw1.wav").Play();
                    break;
                }
        }
    }

    private void PlayEnemyHitSound(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.SLOW:
                {
                    new Sound("fireball.wav").Play();
                    break;
                }
            case EnemyType.FAST:
                {
                    new Sound("ping.wav").Play();
                    break;
                }
            case EnemyType.HOMING:
                {
                    new Sound("bone_throw1.wav").Play();
                    break;
                }
        }
    }

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
            case MyGame.ScreenState.COMIC:
                {
                    _musicChannel = _comicMusic.Play();
                    break;
                }

        }
    }
}

