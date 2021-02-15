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

    public enum PlayerState
    {
        WALKING, IDLE, HURT, DYING
    }
    
    public PlayerState _playerState = PlayerState.IDLE;
    private PlayerAnimations _playerAnimations;
    private byte _playerAnimationTime = 10;
    private float _shieldSpriteOffset = 8;

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
        _playerPosition.x = px;
        _playerPosition.y = py;
        AddChild(new Shield(this));
        AddChild(_playerAnimations = new PlayerAnimations());
        alpha = 0;
        SetPlayerState(PlayerState.IDLE);
        ShowShieldSprites();
    }

    void Update()
    {
        PlayerMovementInputs();
        PlayerMovement();
        UpdatePlayerScreenPosition();
        InvertAnimationSprite();
        RevertPlayerHurt();
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
        _playerPosition += _playerVelocity;
    }

    // moves the player
    void UpdatePlayerScreenPosition()
    {
        x = _playerPosition.x;
        y = _playerPosition.y;
    }

    void OnCollision(GameObject other)
    {
        if (_playerState != PlayerState.HURT 
            && (other is Projectile 
            || other is Enemy))
        {
            other.LateDestroy();
            GameManager.Singleton.PlayerGetsHit(1);
            SetPlayerState(PlayerState.HURT);
        }
    }

    //sets the playerstate and selects the proper animation to play.
    public void SetPlayerState(PlayerState playerstate)
    {
        {
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
                        break;
                    }
            }
        }
    }

    //inverts the animationsprite depending on the direction the player is walking.
    private void InvertAnimationSprite()
    {
        if (_playerDirection.x > 0)
        {
            _playerAnimations.width =  100;
        }else if (_playerDirection.x < 0)
        {
            _playerAnimations.width = -100;
        }
    }


    //loads the shield sprites
    public void ShowShieldSprites()
    {
        AddChild(new ShieldLayer("Paint_layer_2.png", this, _shieldSpriteOffset *2));
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
            Console.WriteLine(_playerState);
            SetPlayerState(PlayerState.IDLE);
        }
    }
}






