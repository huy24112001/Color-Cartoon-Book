using UnityEngine;

namespace MiracleLambda.Utilities
{
	public static class Constants
	{
		// public const string SDK_VERSION = "2.1.1";
		// public const string SDK_SKU_ID = "05";

		// public static class Path
		// {
		// 	public const string CONFIG_FILE = "MapboxConfiguration.txt";
		// 	public const string SCENELIST = "Assets/Mapbox/Resources/Mapbox/ScenesList.asset";
		// 	public static readonly string MAPBOX_RESOURCES_RELATIVE = System.IO.Path.Combine("Mapbox", "MapboxConfiguration");
		// 	public static readonly string MAPBOX_RESOURCES_ABSOLUTE = System.IO.Path.Combine(System.IO.Path.Combine(Application.dataPath, "Resources"), "Mapbox");

		// 	public static readonly string MAPBOX_USER = System.IO.Path.Combine("Assets", System.IO.Path.Combine("Mapbox", "User"));
		// 	public static readonly string MAPBOX_USER_MODIFIERS = System.IO.Path.Combine(MAPBOX_USER, "Modifiers");

		// 	public static readonly string MAP_FEATURE_STYLES_DEFAULT_STYLE_ASSETS = System.IO.Path.Combine("MapboxStyles", "DefaultStyleAssets");
		// 	public static readonly string MAP_FEATURE_STYLES_SAMPLES = System.IO.Path.Combine(System.IO.Path.Combine("MapboxStyles", "Styles"), "MapboxSampleStyles");

		// 	public const string MAPBOX_STYLES_ASSETS_FOLDER = "Assets";
		// 	public const string MAPBOX_STYLES_ATLAS_FOLDER = "Atlas";
		// 	public const string MAPBOX_STYLES_MATERIAL_FOLDER = "Materials";
		// 	public const string MAPBOX_STYLES_PALETTES_FOLDER = "Palettes";
		// }

		/// <summary>
		/// Store common vector constants to avoid the method access cost of Unity's convenience getters.
		/// </summary>
		public static class Math
		{
			public static readonly Vector2 Vector2Zero = new Vector2(0, 0);
			public static readonly Vector2 Vector2One = new Vector2(1, 1);
			public static readonly Vector2 Vector2Up = new Vector2(0, 1);
			public static readonly Vector2 Vector2Down = new Vector2(0, -1);
			public static readonly Vector2 Vector2Right = new Vector2(1, 0);
			public static readonly Vector2 Vector2Left = new Vector2(-1, 0);
			public static readonly Vector2 Vector2Unused = new Vector2(float.MinValue, float.MinValue);

			public static readonly Vector3 Vector3Zero = new Vector3(0, 0, 0);
			public static readonly Vector3 Vector3One = new Vector3(1, 1, 1);
			public static readonly Vector3 Vector3Up = new Vector3(0, 1, 0);
			public static readonly Vector3 Vector3Down = new Vector3(0, -1, 0);
			public static readonly Vector3 Vector3Forward = new Vector3(0, 0, 1);
			public static Vector3 Vector3Right = new Vector3(1, 0, 0);
			public static Vector3 Vector3Left = new Vector3(-1, 0, 0);
			public static readonly Vector3 Vector3Unused = new Vector3(float.MinValue, float.MinValue, float.MinValue);
		}
	}
}
