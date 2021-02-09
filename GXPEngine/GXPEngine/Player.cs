using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

class Player : Sprite
{
	Vec2 _playerVelocity;
	float playerSpeed = 5;
	Vec2 _playerPosition;
	Vec2 _playerDirection;

	Vec2 up;
	Vec2 down;
	Vec2 left;
	Vec2 right;
	public Vec2 Position
	{
		get
		{
			return _playerPosition;
		}
	}

	public Player(float px, float py) : base("circle.png")
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
		_playerVelocity = _playerDirection * playerSpeed;
		_playerPosition += _playerVelocity;
	}

	// moves the player
	void UpdatePlayerScreenPosition()
	{
		x = _playerPosition.x;
		y = _playerPosition.y;
	}
}




