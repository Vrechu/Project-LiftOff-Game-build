using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

class Player : Sprite
{
    private Vec2 _playerPosition;
    private Vec2 _playerDirection;
    private float playerSpeed = 5;
    private Vec2 _playerVelocity;
    private float _inertiaCoefficient = 1.2f;

    private Vec2 up;
    private Vec2 down;
    private Vec2 left;
    private Vec2 right;

    private bool CanMoveX;
    private bool CanMoveY;

    private PlayerAnimations _playerAnimations;
    private byte _playerAnimationTime = 10;
    int _hurtAnimationLoops = 0;
    int _maxHurtAnimationLoops = 1;
    private float _shieldSpriteOffset = 8;

    public static event Action OnDeathAnimationEnd;
    private Sprite dropShadow;

    public enum PlayerState
    {
        WALKING, IDLE, HURT, DYING
    }
    public PlayerState _playerState = PlayerState.IDLE;

    public Vec2 Position
    {
        get
        {
            return _playerPosition;
        }
    }

    public Player(float px, float py) : base("square.png")
    {
        SetOrigin(this.width / 2, this.height / 2);
        x = px;
        y = py;
        AddChild(new Shield(this));
        SetDropShadow("DropShadow.png", 100, 25, 0, 60);
        AddChild(_playerAnimations = new PlayerAnimations());
        alpha = 0;
        SetPlayerState(PlayerState.IDLE);
        ShowShieldSprites();
        GameManager.OnPlayerDeath += DeathAnimation;
    }

    void OnDestroy()
    {
        GameManager.OnPlayerDeath -= DeathAnimation;
    }

    void Update()
    {
        PlayerMovementInputs();
        PlayerMovement();
        UpdatePlayerScreenPosition();
        InvertAnimationSprite();
        RevertPlayerHurt();
        DeathAnimationEnd();
    }

    // processes movement imputs
    void PlayerMovementInputs()
    {
        up.SetXY(0, 0);
        down.SetXY(0, 0);
        left.SetXY(0, 0);
        right.SetXY(0, 0);

        if (Input.GetKey(Key.W))
        {
            up.SetXY(0, -1);
        }
        if (Input.GetKey(Key.S))
        {
            down.SetXY(0, 1);
        }
        if (Input.GetKey(Key.A))
        {
            left.SetXY(-1, 0);
        }
        if (Input.GetKey(Key.D))
        {
            right.SetXY(1, 0);
        }
    }

    // sets calculates player velocity
    void PlayerMovement()
    {
        _playerDirection = up + down + left + right;
        _playerDirection.Normalize();

        if (_playerState != PlayerState.DYING)
        {
            if (_playerDirection.Length() == 0.0)
            {
                if (_playerState != PlayerState.HURT)
                {
                    SetPlayerState(PlayerState.IDLE);
                }
                _playerVelocity = _playerVelocity / _inertiaCoefficient;
            }
            else
            {
                if (_playerState != PlayerState.HURT)
                {
                    SetPlayerState(PlayerState.WALKING);
                }
                _playerVelocity = _playerDirection * playerSpeed;           
            }
        }
    }

    // moves the player
    void UpdatePlayerScreenPosition()
    {
        MoveUntilCollision(_playerVelocity.x, _playerVelocity.y, game.FindObjectsOfType<Wall>());

        _playerPosition.x = x;
        _playerPosition.y = y;
    }

    void OnCollision(GameObject other)
    {
        if (_playerState != PlayerState.HURT
                && _playerState != PlayerState.DYING
            && (other is Projectile))
        {
            Projectile projectile = other as Projectile;
            projectile.StartExploding();
            GameManager.Singleton.PlayerGetsHit(1);
            SetPlayerState(PlayerState.HURT);
        }      
        
    }

    //sets the playerstate and selects the proper animation to play.
    public void SetPlayerState(PlayerState playerstate)
    {
        {
            /*if (_playerState != playerstate)
            {
                Console.WriteLine(playerstate);
            }*/
            _playerState = playerstate;
            switch (_playerState)
            {
                case PlayerState.IDLE:
                    {
                        _playerAnimations.SetCycle(14, 1, _playerAnimationTime);
                        break;
                    }
                case PlayerState.WALKING:
                    {
                        _playerAnimations.SetCycle(14, 6, _playerAnimationTime);
                        break;
                    }
                case PlayerState.HURT:
                    {
                        _playerAnimations.SetCycle(7, 4, _playerAnimationTime);
                        break;
                    }
                case PlayerState.DYING:
                    {
                        _playerAnimations.SetCycle(0, 7, _playerAnimationTime);
                        break;
                    }
            }
        }
    }

    //inverts the animationsprite depending on the direction the player is walking.
    private void InvertAnimationSprite()
    {
        if (_playerVelocity.x > 0)
        {
            _playerAnimations.width = 100;
        }
        else if (_playerVelocity.x < 0)
        {
            _playerAnimations.width = -100;
        }
    }


    //loads the shield sprites
    public void ShowShieldSprites()
    {
        AddChild(new ShieldLayer("Paint_layer_2.png", this, _shieldSpriteOffset * 2));
        AddChild(new ShieldLayer("Paint_layer_3.png", this, _shieldSpriteOffset));
        AddChild(new ShieldLayer("Paint_layer_4.png", this, 0));
        AddChild(new ShieldLayer("Paint_layer_3.png", this, _shieldSpriteOffset * -1));
        AddChild(new ShieldLayer("Paint_layer_2.png", this, _shieldSpriteOffset * -2));
    }

    // reverts the playerstate to idle after the hurt animation is finished
    private void RevertPlayerHurt()
    {
        if (_playerState == PlayerState.HURT
            && _playerAnimations.currentFrame == 10)
        {
            _hurtAnimationLoops++;
        }
        if (_maxHurtAnimationLoops < _hurtAnimationLoops / _playerAnimationTime)
        {
            SetPlayerState(PlayerState.IDLE);
            _hurtAnimationLoops = 0;
        }
    }

    private void DeathAnimation()
    {
        SetPlayerState(PlayerState.DYING);
    }

    private void DeathAnimationEnd()
    {
        if (_playerState == PlayerState.DYING
            && _playerAnimations.currentFrame == 6)
        {
            OnDeathAnimationEnd?.Invoke();
        }
    }

    protected void SetDropShadow(string shadowSprite, int spriteWidth, int spriteHeight, int xOffset, int yOffset)
    {
        dropShadow = new Sprite(shadowSprite);
        dropShadow.SetOrigin(dropShadow.width / 2, dropShadow.height / 2);
        dropShadow.width = spriteWidth;
        dropShadow.height = spriteHeight;
        dropShadow.SetXY(xOffset, yOffset);
        AddChild(dropShadow);
    }
}