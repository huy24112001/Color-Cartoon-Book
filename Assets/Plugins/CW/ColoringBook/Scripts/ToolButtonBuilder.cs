using UnityEngine;

namespace PaintIn2D.ColoringBook
{
	/// <summary>This component spawns <b>ToolButton</b> prefabs based on the specified color list.
	/// This can be used to quickly build scenes that have different color options.</summary>
	public class ToolButtonBuilder : MonoBehaviour
	{
		/// <summary>Each color will be based on this prefab.</summary>
		public ToolButton Prefab { set { prefab = value; } get { return prefab; } } [SerializeField] private ToolButton prefab;

		/// <summary>The angle applied to the spawned prefabs in degrees.</summary>
		public float Angle { set { angle = value; } get { return angle; } } [SerializeField] private float angle;

		/// <summary>This many prefabs will be spawned, and be given these colors.</summary>
		public Color[] ColorOptions { set { colorOptions = value; } get { return colorOptions; } } [SerializeField] private Color[] colorOptions;

		/// <summary>This color index will be selected by default.
		/// -1 = None.</summary>
		public int DefaultColorIndex { set { defaultColorIndex = value; } get { return defaultColorIndex; } } [SerializeField] private int defaultColorIndex;

		protected virtual void Start()
		{
			if (colorOptions != null && prefab != null)
			{
				for (var i = 0; i < colorOptions.Length; i++)
				{
					var clone = Instantiate(prefab, Vector3.zero, Quaternion.Euler(0.0f, 0.0f, angle));

					clone.transform.SetParent(transform, false);

					clone.Color = colorOptions[i];

					if (i == defaultColorIndex)
					{
						var stb = clone.GetComponent<ToolButton>();

						if (stb != null)
						{
							stb.DoSelect();
						}
					}
				}
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn2D.ColoringBook
{
	using CW.Common;
	using UnityEditor;
	using TARGET = ToolButtonBuilder;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class ToolButtonBuilder_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			BeginError(Any(tgts, t => t.Prefab == null));
				Draw("prefab", "Each color will be based on this prefab.");
			EndError();
			Draw("angle", "The angle applied to the spawned prefabs in degrees.");
			Draw("colorOptions", "This many prefabs will be spawned, and be given these colors.");
			Draw("defaultColorIndex", "This color index will be selected by default.\n\n-1 = None.");
		}
	}
}
#endif