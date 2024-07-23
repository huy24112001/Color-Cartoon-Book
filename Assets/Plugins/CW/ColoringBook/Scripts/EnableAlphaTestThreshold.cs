using UnityEngine;
using UnityEngine.UI;
using CW.Common;

namespace PaintIn2D.ColoringBook
{
	/// <summary>This component allows you to change a UI element's hitbox to use its graphic Image opacity/alpha.</summary>
	[ExecuteInEditMode]
	[RequireComponent(typeof(Image))]
	[HelpURL(CwCommon.HelpUrlPrefix + "EnableAlphaTestThreshold")]
	[AddComponentMenu(CwCommon.ComponentMenuPrefix + "Enable AlphaTest Threshold")]
	public class EnableAlphaTestThreshold : MonoBehaviour
	{
		/// <summary>The alpha threshold specifies the minimum alpha a pixel must have for the event to be considered a "hit" on the Image.</summary>
		public float Threshold { set { threshold = value; UpdateThreshold(); } get { return threshold; } } [SerializeField] private float threshold = 0.5f;

		[System.NonSerialized]
		private Image cachedImage;

		[System.NonSerialized]
		private bool cachedImageSet;

		public Image CachedImage
		{
			get
			{
				if (cachedImageSet == false)
				{
					cachedImage    = GetComponent<Image>();
					cachedImageSet = true;
				}

				return cachedImage;
			}
		}

		public void UpdateThreshold()
		{
			CachedImage.alphaHitTestMinimumThreshold = threshold;
		}

		protected virtual void Start()
		{
			UpdateThreshold();
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn2D.ColoringBook
{
	using UnityEditor;
	using TARGET = EnableAlphaTestThreshold;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class EnableAlphaTestThreshold_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			if (Draw("threshold", "The alpha threshold specifies the minimum alpha a pixel must have for the event to be considered a 'hit' on the Image.") == true)
			{
				Each(tgts, t => t.UpdateThreshold(), true);
			}
		}
	}
}
#endif