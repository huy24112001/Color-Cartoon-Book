using CW.Common;
using UnityEngine;

namespace PaintIn2D.ColoringBook
{
	/// <summary>This component aligns and scales the current GameObject to match the specified UI RectTransform.</summary>
	[ExecuteInEditMode]
	public class ColoringBook : MonoBehaviour
	{
		/// <summary>The current outline sprite you will paint.</summary>
		public Sprite Outline { set { outline = value; } get { return outline; } } [SerializeField] private Sprite outline;

		/// <summary>The width & height of the paint canvas.
		/// NOTE: If you have a <b>Outline</b> sprite set, then this will be overridden.</summary>
		public Vector2Int OutlineSize { set { outlineSize = value; } get { return outlineSize; } } [SerializeField] private Vector2Int outlineSize = new Vector2Int(1024, 768);

		/// <summary>The coloring book sprite will be aligned to match this UI <b>RectTransform</b>.</summary>
		public RectTransform Shape { set { shape = value; } get { return shape; } } [SerializeField] private RectTransform shape;

		/// <summary>The Camera used to render this object.
		/// None = Camera.main.</summary>
		public Camera WorldCamera { set { worldCamera = value; } get { return worldCamera; } } [SerializeField] private Camera worldCamera;

		public SpriteRenderer PaintableRenderer { set { paintableRenderer = value; } get { return paintableRenderer; } } [SerializeField] private SpriteRenderer paintableRenderer;

		public SpriteRenderer OutlineRenderer { set { outlineRenderer = value; } get { return outlineRenderer; } } [SerializeField] private SpriteRenderer outlineRenderer;

		/// <summary>This is used by the <b>GetCopyOfCurrentPaintedTextureWithOutline</b> function to create the composite texture.</summary>
		public Material CompositeMaterial { set { compositeMaterial = value; } get { return compositeMaterial; } } [SerializeField] private Material compositeMaterial;

		private static Vector3[] corners = new Vector3[4];

		[ContextMenu("Clear Paint")]
		public void ClearPaint()
		{
			if (paintableRenderer != null)
			{
				var pst = paintableRenderer.GetComponent<CwPaintableSpriteTexture>();

				if (pst != null)
				{
					pst.Clear();
				}
			}
		}

		/// <summary>This function will return a new Texture2D of the current texture the user has painted.</summary>
		public Texture2D GetCopyOfCurrentPaintedTexture()
		{
			if (paintableRenderer != null)
			{
				var pst = paintableRenderer.GetComponent<CwPaintableSpriteTexture>();

				if (pst != null)
				{
					return pst.GetReadableCopy();
				}
			}

			return null;
		}

		/// <summary>This function will return a new Texture2D of the current texture the user has painted with the outline on top.</summary>
		public Texture2D GetCopyOfCurrentPaintedTextureWithOutline()
		{
			if (paintableRenderer != null && outlineRenderer)
			{
				var pst = paintableRenderer.GetComponent<CwPaintableSpriteTexture>();
				var ors = outlineRenderer.sprite;

				if (pst != null && ors != null && ors.texture != null)
				{
					var oldActive = RenderTexture.active;
					var composite = new RenderTexture(pst.Current.descriptor);

					Graphics.Blit(pst.Current, composite);
					Graphics.Blit(ors.texture, composite, compositeMaterial);

					RenderTexture.active = oldActive;

					var copy = PaintCore.CwCommon.GetReadableCopy(composite);

					DestroyImmediate(composite);

					return copy;
				}
			}

			return null;
		}

		public void UpdateAlignment()
		{
			var camera = worldCamera != null ? worldCamera : Camera.main;

			var bounds = (Vector2)outlineSize;

			if (outline != null)
			{
				bounds = outline.bounds.size;
			}

			if (shape != null && camera != null)
			{
				shape.GetWorldCorners(corners);

				var size   = camera.ScreenToWorldPoint(corners[2] - corners[0]) - camera.ScreenToWorldPoint(Vector3.zero);
				var scale  = CwHelper.Reciprocal(bounds.y) * size.y;
				var aspect = bounds.x / bounds.y;

				if (aspect > 0.0f)
				{
					var ratio = CwHelper.Divide(size.x, size.y);

					if (ratio < aspect)
					{
						scale *= ratio / aspect;
					}
				}

				transform.position   = camera.ScreenToWorldPoint((corners[0] + corners[2]) * 0.5f);
				transform.rotation   = camera.transform.rotation;
				transform.localScale = new Vector3(scale, scale, 1.0f);

				var px = CwHelper.Divide(bounds.x, paintableRenderer.sprite.bounds.size.x);
				var py = CwHelper.Divide(bounds.y, paintableRenderer.sprite.bounds.size.y);

				paintableRenderer.transform.localScale = new Vector3(px, py, 1.0f);
			}
		}

		protected virtual void Update()
		{
			if (outlineRenderer != null)
			{
				outlineRenderer.sprite = outline;
			}

			if (outline != null)
			{
				outlineSize = new Vector2Int(outline.texture.width, outline.texture.height);
			}

			UpdateAlignment();
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn2D.ColoringBook
{
	using UnityEditor;
	using TARGET = ColoringBook;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class AlignToCanvas_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("outline", "The current outline sprite you will paint.");
			BeginDisabled(Any(tgts, t => t.Outline != null));
				Draw("outlineSize", "The width & height of the paint canvas.\r\n\t\t/// NOTE: If you have a <b>Outline</b> sprite set, then this will be overridden.");
			EndDisabled();

			Separator();

			BeginError(Any(tgts, t => t.Shape == null));
				Draw("shape", "The coloring book sprite will be aligned to match this UI <b>RectTransform</b>.");
			EndError();
			Draw("worldCamera", "The Camera used to render this object.\n\nNone = Camera.main.");
			Draw("paintableRenderer");
			Draw("outlineRenderer");
			Draw("compositeMaterial", "This is used by the <b>GetCopyOfCurrentPaintedTextureWithOutline</b> function to create the composite texture.");
		}
	}
}
#endif