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

	/// <summary>
	/// calculates the lenght of the vector
	/// </summary>
	public float Length()
	{
		return Mathf.Sqrt((this.x * this.x) + (this.y * this.y));
	}

	/// <summary>
	/// scales the vector length to 1 without changing the direction
	/// </summary>
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
	/// <summary>
	/// returns a new vector with length 1 in the same direction od the original 
	/// </summary>
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

	/// <summary>
	/// changes the x and y coördinates of the vector
	/// </summary>
	public void SetXY(float _x, float _y)
	{
		this.x = _x;
		this.y = _y;
	}

	/// <summary>
	/// converts angles from degrees to radians
	/// </summary>
	public float Deg2Rad(float _degree)
	{
		return (_degree * (Mathf.PI / 180));
	}
	/// <summary>
	/// converts angles from radians to degrees
	/// </summary>
	public float Rad2Deg(float _radian)
	{
		return (_radian * (180 / Mathf.PI));
	}

	/// <summary>
	/// returns a vector with length 1 and the specified angle
	/// </summary>
	public Vec2 GetUnitVectorDeg(float degree)
	{
		return new Vec2(Mathf.Cos(Deg2Rad(degree)), Mathf.Sin(Deg2Rad(degree)));
	}
	/// <summary>
	/// returns a vector with length 1 and the specified angle
	/// </summary>
	public Vec2 GetUnitVectorRad(float radian)
	{
		return new Vec2(Mathf.Cos(radian), Mathf.Sin(radian));
	}

	/// <summary>
	/// returns a vector with lenght 1 and a random angle
	/// </summary>
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

	/// <summary>
	/// sets the vector to the specified angle
	/// </summary>
	public void SetAngleRadians(float radian)
	{
		var length = Length();
		SetXY(Mathf.Cos(radian) * length, Mathf.Sin(radian) * length);
	}
	/// <summary>
	/// sets the vector to the specified angle
	/// </summary>
	public void SetAngleDegrees(float degree)
	{
		var length = Length();
		SetXY(Mathf.Cos(Deg2Rad(degree)) * length, Mathf.Sin(Deg2Rad(degree)) * length);
	}

	/// <summary>
	/// returns the angle of the vector
	/// </summary>
	public float GetAngleDegrees()
	{
		return Rad2Deg(Mathf.Atan2(this.y, this.x));
	}
	/// <summary>
	/// returns the angle of the vector
	/// </summary>
	public float GetAngleRadians()
	{
		return Mathf.Atan2(this.y, this.x);
	}

	/// <summary>
	/// rotates the vector over the specified angle
	/// </summary>
	public void RotateDegrees(float degree)
	{
		float angleSine = Mathf.Sin(Deg2Rad(degree));
		float angleCosine = Mathf.Cos(Deg2Rad(degree));
		SetXY(this.x * angleCosine - this.y * angleSine, this.x * angleSine + this.y * angleCosine);
	}
	/// <summary>
	/// rotates the vector over the specified angle
	/// </summary>
	public void RotateRadians(float radian)
	{
		float angleSine = Mathf.Sin(radian);
		float angleCosine = Mathf.Cos(radian);
		SetXY(this.x * angleCosine - this.y * angleSine, this.x * angleSine + this.y * angleCosine);
	}

	/// <summary>
	/// rotates the vector around the specified point over the specified angle
	/// </summary>
	public void RotateAroundDegrees(Vec2 point, float degree)
	{
		this -= point;
		this.RotateDegrees(degree);
		this += point;
	}
	/// <summary>
	/// rotates the vector around the specified point over the specified angle
	/// </summary>
	public void RotateAroundRadians(Vec2 point, float radian)
	{
		this -= point;
		this.RotateRadians(radian);
		this += point;
	}

	/// <summary>
	/// returns the dot product of two vectors
	/// </summary>
	public float Dot(Vec2 other)
	{
		return this.x * other.x + this.y * other.y;
	}

	/// <summary>
	/// returns a normal with length 1 of the vector
	/// </summary>
	public Vec2 UnitNormal()
	{
		Vec2 normal = new Vec2(this.y * -1, this.x);
		normal.Normalize();
		return normal;
	}

	/// <summary>
	/// changes the vector angle to the reflected version of the angle compared to a different vector. the "bounciness" of the vector is specified with the COR from 0 to 1
	/// </summary>
	public void Reflect(Vec2 other, float COR)
	{
		this -= (1 + COR) * (this.Dot(other.UnitNormal())) * other.UnitNormal();
	}
}