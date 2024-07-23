using UnityEngine;

namespace PaintIn2D.ColoringBook
{
	/// <summary>This component spawns <b>ColorButton</b> prefabs based on the specified color list.
	/// This can be used to quickly build scenes that have different color options.</summary>
	public class ColorButtonBuilder : MonoBehaviour
	{
		/// <summary>Each color will be based on this prefab.</summary>
		public ColorButton Prefab { set { prefab = value; } get { return prefab; } } [SerializeField] private ColorButton prefab;

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
					var clone = Instantiate(prefab);

					clone.Color = colorOptions[i];

					clone.transform.SetParent(transform, false);

					if (i == defaultColorIndex)
					{
						clone.DoSelect();
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
	using TARGET = ColorButtonBuilder;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class ColorButtonBuilder_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			BeginError(Any(tgts, t => t.Prefab == null));
				Draw("prefab", "Each color will be based on this prefab.");
			EndError();
			Draw("colorOptions", "This many prefabs will be spawned, and be given these colors.");
			Draw("defaultColorIndex", "This color index will be selected by default.\n\n-1 = None.");
		}
	}
}
#endif