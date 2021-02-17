using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

class Player : Sprite
{
    private Vec2 _playerPosition;
    private Vec2 _playerDirection;
    private float playerSpeed = 4;          // player movement speed
    private Vec2 _playerVelocity;
    private float _inertiaCoefficient = 1.2f;   //time it takes the player to slow down

    private Vec2 up;
    private Vec2 down;
    private Vec2 left;
    private Vec2 right;

    private PlayerAnimations _playerAnimations;     // animation sprite of the player
    private byte _playerAnimationTime = 10;         //time each animation frame is shown
    int _hurtAnimationLoops = 0;            
    int _maxHurtAnimationLoops = 1;             //amount of time the hurt animation loops
    private float _shieldSpriteOffset = 8;          //the offset of the shield sprites

    public static event Action OnDeathAnimationEnd;         //death animation end event
    private Sprite dropShadow;              //shadow sprite

    public static event Action OnPLayerHit;

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
        MoveUntilCollision(_playerVelocity.x, _playerVelocity.y, dropShadow, game.FindObjectsOfType<Wall>());

        _playerPosition.x = x;
        _playerPosition.y = y;
    }

    //collision method
    void OnCollision(GameObject other)
    {
        if (_playerState != PlayerState.HURT            //if player is not in hurt or dying state
                && _playerState != PlayerState.DYING
            && (other is Projectile))
        {
            Projectile projectile = other as Projectile;        //set projectile to exploding
            projectile.StartExploding();
            GameManager.Singleton.PlayerGetsHit(1);             //player gets hit and loses health
            SetPlayerState(PlayerState.HURT);               //playerstate is hurt
            OnPLayerHit?.Invoke();
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

    //sets the nplayer state to dying
    private void DeathAnimation()
    {
        SetPlayerState(PlayerState.DYING);
    }

    //invokes the event of the deathanimation ending
    private void DeathAnimationEnd()
    {
        if (_playerState == PlayerState.DYING
            && _playerAnimations.currentFrame == 6)
        {
            OnDeathAnimationEnd?.Invoke();
        }
    }

    //places the dropshadow of the player
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