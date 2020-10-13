using System.Collections;
using System.Collections.Generic;

static class Vector3Extensions
{
	public static UnityEngine.Vector3 ToUnity(this System.Numerics.Vector3 v)
	{
		return new UnityEngine.Vector3(v.X, v.Y, v.Z);
	}
	public static UnityEngine.Vector3 ToUnity(this UnityEngine.Vector3d v)
	{
		return new UnityEngine.Vector3((float)v.x, (float)v.y, (float)v.z);
	}

	public static System.Numerics.Vector3 ToRegular(this UnityEngine.Vector3 v)
	{
		return new System.Numerics.Vector3(v.x, v.y, v.z);
	}
	public static System.Numerics.Vector3 ToRegular(this UnityEngine.Vector3d v)
	{
		return new System.Numerics.Vector3((float)v.x, (float)v.y, (float)v.z);
	}
	
	public static UnityEngine.Vector3d ToUnityd(this UnityEngine.Vector3 v)
	{
		return new UnityEngine.Vector3d(v.x, v.y, v.z);
	}
	public static UnityEngine.Vector3d ToUnityd(this System.Numerics.Vector3 v)
	{
		return new UnityEngine.Vector3d(v.X, v.Y, v.Z);
	}
}