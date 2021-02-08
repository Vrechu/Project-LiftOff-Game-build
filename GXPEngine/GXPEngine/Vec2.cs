using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using GXPEngine;
using GXPEngine.Core;

//--------------------------------------------------------------------------------------------------
//                                         VECTOR STRUCT
//--------------------------------------------------------------------------------------------------
// my finished vector struct
public struct Vec2
{
	//vector's coördinates
	public float x;
	public float y;

	public Vec2(float pX = 0, float pY = 0)
	{
		x = pX;
		y = pY;
	}

	//vector addition
	public static Vec2 operator +(Vec2 left, Vec2 right)
	{
		return new Vec2(left.x + right.x, left.y + right.y);
	}

	//vector subtracion
	public static Vec2 operator -(Vec2 left, Vec2 right)
	{
		return new Vec2(left.x - right.x, left.y - right.y);
	}

	//vector scaling
	public static Vec2 operator *(Vec2 left, float right)
	{
		return new Vec2(left.x * right, left.y * right);
	}
	public static Vec2 operator *(float left, Vec2 right)
	{
		return new Vec2(left * right.x, left * right.y);
	}

	//vector division
	public static Vec2 operator /(Vec2 left, float right)
	{
		return new Vec2(left.x / right, left.y / right);
	}

	//calculates the lenght of the vector
	public float Length()
	{
		return Mathf.Sqrt((this.x * this.x) + (this.y * this.y));
	}

	//scales the vector length to 1 without changing the direction
	public void Normalize()
	{
		if (Length() == 0)
		{
			this *= 0;
		}
		if (Length() != 0)
		{
			this *= 1 / Length();
		}
	}

	//returns a new vector with length 1 in the same direction od the original 
	public Vec2 Normalized()
	{
		if (Length() != 0)
		{
			Vec2 normalizedVec = this;
			normalizedVec.Normalize();
			return normalizedVec;
		}
		else
		{
			return new Vec2(0, 0);
		}
	}

	public override string ToString()
	{
		return String.Format("({0},{1})", x, y);
	}

	//changes the x and y coördinates of the vector
	public void SetXY(float _x, float _y)
	{
		this.x = _x;
		this.y = _y;
	}

	//converts angles from degrees to radians or from radians to degrees
	public float Deg2Rad(float _degree)
	{
		return (_degree * (Mathf.PI / 180));
	}
	public float Rad2Deg(float _radian)
	{
		return (_radian * (180 / Mathf.PI));
	}

	//returns a vector with length 1 and the specified angle
	public Vec2 GetUnitVectorDeg(float degree)
	{
		return new Vec2(Mathf.Cos(Deg2Rad(degree)), Mathf.Sin(Deg2Rad(degree)));
	}
	public Vec2 GetUnitVectorRad(float radian)
	{
		return new Vec2(Mathf.Cos(radian), Mathf.Sin(radian));
	}

	//returns a vector with lenght 1 and a random angle
	public Vec2 RandomUnitVector()
	{
		float randomAngle;
		var rand = new Random();
		var randomNumber = (float)rand.NextDouble() * 2;
		if (randomNumber > 1)
		{
			randomNumber = -1 + (randomNumber - 1);
		}
		randomAngle = Mathf.PI * randomNumber;
		return GetUnitVectorRad(randomAngle);
	}

	//sets the vector to the specified angle
	public void SetAngleRadians(float radian)
	{
		var length = Length();
		SetXY(Mathf.Cos(radian) * length, Mathf.Sin(radian) * length);
	}
	public void SetAngleDegrees(float degree)
	{
		var length = Length();
		SetXY(Mathf.Cos(Deg2Rad(degree)) * length, Mathf.Sin(Deg2Rad(degree)) * length);
	}

	//returns the angle of the vector
	public float GetAngleDegrees()
	{
		return Rad2Deg(Mathf.Atan2(this.y, this.x));
	}
	public float GetAngleRadians()
	{
		return Mathf.Atan2(this.y, this.x);
	}

	//rotates the vector over the specified angle
	public void RotateDegrees(float degree)
	{
		float angleSine = Mathf.Sin(Deg2Rad(degree));
		float angleCosine = Mathf.Cos(Deg2Rad(degree));
		SetXY(this.x * angleCosine - this.y * angleSine, this.x * angleSine + this.y * angleCosine);
	}
	public void RotateRadians(float radian)
	{
		float angleSine = Mathf.Sin(radian);
		float angleCosine = Mathf.Cos(radian);
		SetXY(this.x * angleCosine - this.y * angleSine, this.x * angleSine + this.y * angleCosine);
	}

	//rotates the vector around the specified point over the specified angle
	public void RotateAroundDegrees(Vec2 point, float degree)
	{
		this -= point;
		this.RotateDegrees(degree);
		this += point;
	}
	public void RotateAroundRadians(Vec2 point, float radian)
	{
		this -= point;
		this.RotateRadians(radian);
		this += point;
	}

	//returns the dot product of two vectors
	public float Dot(Vec2 other)
	{
		return this.x * other.x + this.y * other.y;
	}

	//returns a normal with length 1 of the vector 
	public Vec2 UnitNormal()
	{
		Vec2 normal = new Vec2(this.y * -1, this.x);
		normal.Normalize();
		return normal;
	}

	//changes the vector angle to the reflected version of the angle compared to a different vector
	//the "bounciness" of the vector is specified with the COR from 0 to 1
	public void Reflect(Vec2 other, float COR)
	{
		this -= (1 + COR) * (this.Dot(other.UnitNormal())) * other.UnitNormal();
	}
}