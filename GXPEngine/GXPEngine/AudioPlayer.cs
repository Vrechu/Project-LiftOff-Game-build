using System;
using System.Collections.Generic;
using GXPEngine.Projectiles;
using System.Linq;
using System.Text;
using GXPEngine;

class AudioPlayer : GameObject
{
    MyGame _myGame;
    SoundChannel _musicChannel;
    Sound _music;
    SoundChannel _effectsChannel;
    Sound _effect;

    public AudioPlayer(MyGame myGame)
    {
        _myGame = myGame;
        EventSubscriptions();
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
        _effect = new Sound("character_hit.wav");
        _effectsChannel = _effect.Play();
    }

    private void PlayDeathSound()
    {
        _musicChannel.Stop();
        _effect = new Sound("PlayerDead.wav");
        _effectsChannel = _effect.Play();        
    }

    private void PlayReflectSound()
    {
        _effect = new Sound("reflect_sound.wav");
        _effectsChannel = _effect.Play();
    }

    private void PlayProjectileShotSound(ProjectileType projectileType)
    {
        switch (projectileType)
        {
            case ProjectileType.SLOW:
                {
                    _effect = new Sound("fireball.wav");
                    _effectsChannel = _effect.Play();
                    break;
                }
            case ProjectileType.FAST:
                {
                    _effect = new Sound("ping.wav");
                    _effectsChannel = _effect.Play();
                    break;
                }
            case ProjectileType.HOMING:
                {
                    _effect = new Sound("bone_throw1.wav");
                    _effectsChannel = _effect.Play();
                    break;
                }
        }
    }

    private void PlayEnemyHitSound()
    {
        new Sound("reflect_sound.wav").Play();
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
                    _music = new Sound("test_music.mp3");
                    _musicChannel = _music.Play();
                    break;
                }
            case MyGame.ScreenState.MENU:
                {
                    _music = new Sound("ping.wav");
                    _musicChannel = _music.Play();
                    break;
                }
            case MyGame.ScreenState.COMIC:
                {
                    _music = new Sound("bone_throw1.wav");
                    _musicChannel = _music.Play();
                    break;
                }

        }
    }
}

