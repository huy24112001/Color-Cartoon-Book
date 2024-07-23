namespace MiracleLambda.Utilities
{
	using UnityEngine;

	public static class VectorExtensions
	{
		public static Vector3 ToVector3xz(this Vector2 v)
		{
			return new Vector3(v.x, 0, v.y);
		}

		public static Vector3 ToVector3yz(this Vector2 v)
		{
			return new Vector3(0, v.x, v.y);
		}

		public static Vector3 ToVector3(this Vector2 v)
		{
			return new Vector3(v.x, v.y, 0);
		}

		public static Vector2 ToVector2xz(this Vector3 v)
		{
			return new Vector2(v.x, v.z);
		}

		public static Vector2 ToVector2(this Vector3 v)
		{
			return new Vector2(v.x, v.y);
		}
		
		public static Vector3 Perpendicular(this Vector3 v)
		{
			return new Vector3(-v.z, v.y, v.x);
		}
	}
}
