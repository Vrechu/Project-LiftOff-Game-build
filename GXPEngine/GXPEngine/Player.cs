using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

class Player : AnimationSprite
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
    public Vec2 Position
    {
        get
        {
            return _playerPosition;
        }
    }

    public Player(float px, float py) : base("wizard_walk.png", 6,1,1)
    {
        SetOrigin(this.width / 2, this.height / 2);
        _playerPosition.x = px;
        _playerPosition.y = py;
        AddChild(new Shield(this));
    }

    void Update()
    {
        PlayerMovementInputs();
        PlayerMovement();
        UpdatePlayerScreenPosition();
        /*PlayerDies();*/
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
            _playerVelocity = _playerVelocity / _inertiaCoefficient;
        }
        else
        {
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
        if (other is Projectile)
        {
            other.LateDestroy();
            GameManager.Singleton.PlayerGetsHit(1);
        }
    }

    /*void PlayerDies()
    {
		if (GameManager.Singleton._playerHealth == 0)
        {
			LateDestroy();
        }
    }*/
}




