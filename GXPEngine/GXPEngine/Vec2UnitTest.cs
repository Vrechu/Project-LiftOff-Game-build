using System;
using System.Threading;
using GXPEngine;
//--------------------------------------------------------------------------------------------------
//                                         VECTOR UNIT TESTING CLASS
//--------------------------------------------------------------------------------------------------
// class containing all tests for the vector struct
class Vec2UnitTest : GameObject
{
	//test vectors
	Vec2 testVec1 = new Vec2(2, 4);
	Vec2 testVec2 = new Vec2(5, 10);
	Vec2 testVec3 = new Vec2(3, 6);
	//test angles
	float testDeg1 = new float();
	float testRad1 = new float();

	public Vec2UnitTest()
	{
	}

	void Update()
	{
		//input for doing tests
		if (Input.GetKeyUp(Key.V))
		{
			UnitTests();
		}
	}

	private void UnitTests()
	{
		// all vector method tests

		//testvec 1: x = 2, y = 4
		//testvec 2: x = 5, y = 10
		//testvec 3: x = 3, y = 6
		//testDeg 1: 90 degrees
		//testRad 1: 0,25 PI radians

		FloatIsEqualTest();
		ResetTestValues();
		SestSetXY();
		AddTest();
		SubtractTest();
		ScaleTest();
		LengthTest();
		NormalizeTest();
		NormalizedTest();
		Deg2RadTest();
		Rad2DegTest();
		UnitVectorDegTest();
		UnitVectorRadTest();
		RandomUnitVectorTest();
		GetAngleDegreesTest();
		GetAnglesRadiansTest();
		SetAngleRadiansTest();
		SetAngleDegreesTest();
		RotateDegreesTest();
		RotateRadiansTest();
		RotateAroundDegreesTest();
		RotateAroundRadiansTest();
		DotTest();
		UnitNormalTest();
		ReflectTest();
	}

	private void ResetTestValues()
	{
		//resets the test vectors and angles for later use
		testVec1.SetXY(2, 4);
		testVec2.SetXY(5, 10);
		testVec3.SetXY(3, 6);

		testDeg1 = 90;
		testRad1 = 0.25f * Mathf.PI;
		Console.WriteLine("-");
	}

	bool FloatIsEqual(float left, float right)
	{
		//replacement for "==" since some methods don't return perfectly accurate values 
		return (Mathf.Abs(left - right) < 0.00001);
	}

	void FloatIsEqualTest()
	{
		Console.WriteLine("float is equal ok? :"
			+ FloatIsEqual(2.000001f, 2.000002f));
	}

	private void SestSetXY()
	{
		testVec1.SetXY(5, 7);
		testVec2.SetXY(2, 3);
		testVec3.SetXY(10, 20);
		Console.WriteLine("SetXY ok? :"
			+ (testVec1.x == 5 && testVec1.y == 7
			&& testVec2.x == 2 && testVec2.y == 3
			&& testVec3.x == 10 && testVec3.y == 20));
		ResetTestValues();
	}

	private void AddTest()
	{
		testVec3 = testVec1 + testVec2;
		Console.WriteLine("Addition ok? :"
			+ (testVec1.x == 2 && testVec1.y == 4
			&& testVec2.x == 5 && testVec2.y == 10
			&& testVec3.x == 7 && testVec3.y == 14));
		ResetTestValues();
	}

	private void SubtractTest()
	{
		testVec3 = testVec2 - testVec1;
		Console.WriteLine("Subtraction ok? :"
			+ (testVec1.x == 2 && testVec1.y == 4
			&& testVec2.x == 5 && testVec2.y == 10
			&& testVec3.x == 3 && testVec3.y == 6));
		ResetTestValues();
	}

	private void ScaleTest()
	{
		testVec2 = testVec1 * 2;
		testVec3 = 3 * testVec1;
		Console.WriteLine("Scaling ok? :"
			+ (testVec1.x == 2 && testVec1.y == 4
			&& testVec2.x == 4 && testVec2.y == 8
			&& testVec3.x == 6 && testVec3.y == 12));
		ResetTestValues();
	}

	private void LengthTest()
	{
		Console.WriteLine("Calculate length ok? :"
			+ (testVec1.Length() == Mathf.Sqrt(20)
			&& testVec2.Length() == Mathf.Sqrt(125)));
		ResetTestValues();
	}

	private void NormalizeTest()
	{
		testVec1.Normalize();
		testVec2.SetXY(0, 0);
		testVec2.Normalize();
		Console.WriteLine("Normalize ok? :"
			+ (testVec1.Length() == 1
			&& testVec2.Length() == 0));
		ResetTestValues();
	}

	private void NormalizedTest()
	{
		testVec2.SetXY(0, 0);
		Console.WriteLine("Normalized new Vec ok? :"
			+ (testVec1.Normalized().Length() == 1
			&& testVec2.Normalized().Length() == 0));
		ResetTestValues();
	}

	private void Deg2RadTest()
	{
		Console.WriteLine("Degrees to Radians ok? :"
		+ FloatIsEqual(testVec1.Deg2Rad(testDeg1), 0.5f * Mathf.PI));
		ResetTestValues();
	}

	private void Rad2DegTest()
	{
		Console.WriteLine("Radians to Degrees ok? :"
		+ FloatIsEqual(testVec1.Rad2Deg(testRad1), 45));
		ResetTestValues();
	}

	private void UnitVectorDegTest()
	{
		Console.WriteLine("UnitVector Degrees ok? :"
			+ (testVec1.GetUnitVectorDeg(testDeg1).x == Mathf.Cos(testVec1.Deg2Rad(90))
			&& testVec1.GetUnitVectorDeg(testDeg1).y == Mathf.Sin(testVec1.Deg2Rad(90))
			&& FloatIsEqual(Mathf.Sqrt(Mathf.Pow(testVec1.GetUnitVectorDeg(testVec1.Deg2Rad(testDeg1)).x, 2) +
			Mathf.Pow(testVec1.GetUnitVectorDeg(testVec1.Deg2Rad(testDeg1)).y, 2)), 1f)));
		ResetTestValues();
	}

	private void UnitVectorRadTest()
	{
		Console.WriteLine("UnitVector Radians ok? :"
			+ (testVec1.GetUnitVectorRad(testRad1).x == Mathf.Cos(0.25f * Mathf.PI)
			&& testVec1.GetUnitVectorRad(testRad1).y == Mathf.Sin(0.25f * Mathf.PI)
			&& FloatIsEqual(Mathf.Sqrt(Mathf.Pow(testVec1.GetUnitVectorRad(testRad1).x, 2) +
			Mathf.Pow(testVec1.GetUnitVectorRad(testRad1).y, 2)), 1f)));
		ResetTestValues();
	}

	private void RandomUnitVectorTest()
	{
		Console.WriteLine("Random UnitVector ok? :"
			+ (FloatIsEqual(testVec1.RandomUnitVector().Length(), 1f)));
		ResetTestValues();
	}

	private void GetAngleDegreesTest()
	{
		testVec1.SetXY(5, 5);
		testVec2.SetXY(-5, -5);
		Console.WriteLine("Get Angle Degrees ok? :"
			+ (testVec1.GetAngleDegrees() == 45
			&& testVec2.GetAngleDegrees() == -135));
		ResetTestValues();
	}

	private void GetAnglesRadiansTest()
	{
		testVec1.SetXY(5, 5);
		testVec2.SetXY(-5, -5);
		Console.WriteLine("Get Angle Radians ok? :"
			+ (testVec1.GetAngleRadians() == 0.25 * Mathf.PI
			&& FloatIsEqual(testVec2.GetAngleRadians(), -0.75f * Mathf.PI)));
		ResetTestValues();
	}

	private void SetAngleRadiansTest()
	{
		testVec1.SetAngleRadians(testRad1);
		testVec2.SetAngleRadians(-0.25f * Mathf.PI);
		Console.WriteLine("Set Angle Radians ok? :"
			+ (testVec1.Length() == Mathf.Sqrt(20)
			&& testVec1.GetAngleRadians() == testRad1
			&& FloatIsEqual(testVec2.GetAngleRadians(), -0.25f * Mathf.PI)));
		ResetTestValues();
	}

	private void SetAngleDegreesTest()
	{
		testVec1.SetAngleDegrees(testDeg1);
		testVec2.SetAngleDegrees(-45);
		Console.WriteLine("Set Angle Degrees ok? :"
			+ (testVec1.Length() == Mathf.Sqrt(20)
			&& testVec1.GetAngleDegrees() == testDeg1
			&& FloatIsEqual(testVec2.GetAngleDegrees(), -45)));
		ResetTestValues();
	}

	private void RotateDegreesTest()
	{
		testVec1.SetAngleDegrees(30);
		testVec1.RotateDegrees(50);
		testVec2.SetAngleDegrees(120);
		testVec2.RotateDegrees(-70);
		Console.WriteLine("Rotate Degees ok? :"
			+ (FloatIsEqual(testVec1.GetAngleDegrees(), 80)
			&& FloatIsEqual(testVec2.GetAngleDegrees(), 50)));
		ResetTestValues();
	}

	private void RotateRadiansTest()
	{
		testVec1.SetAngleRadians(0.3f * Mathf.PI);
		testVec1.RotateRadians(0.2f * Mathf.PI);
		testVec2.SetAngleRadians(0.6f * Mathf.PI);
		testVec2.RotateRadians(-0.2f * Mathf.PI);
		Console.WriteLine("Rotate Radians ok? :"
			+ (FloatIsEqual(testVec1.GetAngleRadians(), 0.5f * Mathf.PI)
			&& FloatIsEqual(testVec2.GetAngleRadians(), 0.4f * Mathf.PI)));
		ResetTestValues();
	}

	private void RotateAroundDegreesTest()
	{
		testVec1.SetXY(1, 1);
		testVec2.SetXY(2, 1);
		testVec2.RotateAroundDegrees(testVec1, 90);
		Console.WriteLine("Rotate Around Degrees ok? :" + (FloatIsEqual(testVec2.x, 1)
			&& FloatIsEqual(testVec2.y, 2)));
		ResetTestValues();
	}

	private void RotateAroundRadiansTest()
	{
		testVec1.SetXY(1, 1);
		testVec2.SetXY(2, 1);
		testVec2.RotateAroundRadians(testVec1, Mathf.PI);
		Console.WriteLine("Rotate Around Radians ok? :"
			+ (FloatIsEqual(testVec2.x, 0)
			&& FloatIsEqual(testVec2.y, 1)));
		ResetTestValues();
	}

	private void DotTest()
	{
		testVec1.SetXY(2, 2);
		testVec2.SetXY(3, 3);

		Console.WriteLine("Dot Product ok? :"
			+ (testVec1.Dot(testVec2) == 12));
		ResetTestValues();
	}

	private void UnitNormalTest()
	{
		testVec1.SetXY(5, 0);
		Console.WriteLine("Unit Normal ok? :"
			+ (FloatIsEqual(testVec1.UnitNormal().x, 0)
			&& FloatIsEqual(testVec1.UnitNormal().y, 1)
			&& FloatIsEqual(testVec1.UnitNormal().Length(), 1)));
		ResetTestValues();
	}

	private void ReflectTest()
	{
		testVec1.SetAngleDegrees(45);
		testVec2.SetAngleDegrees(0);
		testVec1.Reflect(testVec2, 1);
		Console.WriteLine("Reflect ok? :"
			+ (FloatIsEqual(testVec1.GetAngleDegrees(), -45)));
		ResetTestValues();
	}
}

