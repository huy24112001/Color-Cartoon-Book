using UnityEngine;
using PaintCore;
using System.Collections.Generic;
using UnityEngine.UI;
using CW.Common;

namespace PaintIn2D.ColoringBook
{
	/// <summary>This component changes the LocalMask of the specified <b>P3dPaintableTexture</b> based on the mouse/finger drawing on top of it.
	/// The mask is set to isolate the current segment being painted, based on the outline of the current coloring book texture.</summary>
	public class ColoringBookMasker : MonoBehaviour
	{
		class Mask
		{
			public float     Age;
			public Texture2D Tex;
			public int       Seg = -1;

			public HashSet<object> Owners = new HashSet<object>();
		}

		public enum UseType
		{
			SegmentFirstUnderFinger,
			SegmentCurrentlyUnderFinger
		}

		/// <summary>The outline is stored in this <b>SpriteRenderer</b>.</summary>
		public SpriteRenderer OutlineRenderer { set { outlineRenderer = value; } get { return outlineRenderer; } } [SerializeField] private SpriteRenderer outlineRenderer;

		/// <summary>Each pixel in the outline must have at least this much alpha opacity for it to be considered part of the outline.</summary>
		public float OutlineThreshold { set { outlineThreshold = value; } get { return outlineThreshold; } } [SerializeField] [Range(0.0f, 1.0f)] private float outlineThreshold = 0.75f;

		/// <summary>The masks will be applied to this <b>P3dPaintableTexture</b>.</summary>
		public CwPaintableTexture TextureToMask { set { textureToMask = value; } get { return textureToMask; } } [SerializeField] private CwPaintableTexture textureToMask;

		/// <summary>How many pixels should the segment borders be extended by? This can remove thin white edges if your outline texture isn't pixel perfect.</summary>
		public int Feather { set { feather = value; } get { return feather; } } [SerializeField] private int feather = 1;

		/// <summary>How should the coloring book outline be masked?
		/// SegmentCurrentlyUnderFinger = The segment will update based on where your finger is.
		/// SegmentFirstUnderFinger = The segment will remain locked to the first one under your finger when you begin touching the screen.</summary>
		public UseType Use { set { use = value; } get { return use; } } [SerializeField] private UseType use;

		private Color32[] pixels;

		private int[] segments;

		private int width;

		private int height;

		private List<Mask> masks = new List<Mask>();

		private Texture2D maskedTexture;

		private Mask GetMask(object owner, int segment)
		{
			if (use == UseType.SegmentFirstUnderFinger)
			{
				foreach (var mask in masks)
				{
					if (mask.Owners.Contains(owner) == true)
					{
						return mask;
					}
				}
			}

			if (use == UseType.SegmentCurrentlyUnderFinger)
			{
				foreach (var mask in masks)
				{
					mask.Owners.Remove(owner);
				}
			}

			foreach (var mask in masks)
			{
				if (mask.Seg == segment)
				{
					mask.Owners.Add(owner);

					return mask;
				}
			}

			return CreateMask(segment, owner);
		}

		private Mask CreateMask(int segment, object owner)
		{
			var newMask = new Mask();

			ReplaceMask(newMask, segment);

			masks.Add(newMask);

			newMask.Owners.Add(owner);

			return newMask;
		}

		private void ReplaceMask(Mask mask, int segment)
		{
			var c = default(Color32);

			for (var i = 0; i < pixels.Length; i++)
			{
				var s = segments[i];

				if (s == segment)
				{
					c.a = 255;
				}
				else
				{
					c.a = 0;
				}

				pixels[i] = c;
			}

			if (mask.Tex == null)
			{
				mask.Tex = new Texture2D(width, height, TextureFormat.Alpha8, false);
			}

			mask.Tex.SetPixels32(pixels);
			mask.Tex.Apply();

			mask.Seg = segment;
		}

		public void ApplyMask(CwCommandDecal2D commandDecal2D, int segment)
		{
			var mask = GetMask(CwPaintableManager.LastPaintingObject, segment);

			mask.Age = 0;

			commandDecal2D.LocalMaskTexture = mask.Tex;
			commandDecal2D.LocalMaskChannel = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
		}

		private int GetSegment(int x, int y)
		{
			if (x >= 0 && x < width && y >= 0 && y < height)
			{
				return segments[x + y * width];
			}

			return 0;
		}

		protected virtual void Start()
		{
			CheckMask();
		}

		protected virtual void OnEnable()
		{
			if (textureToMask != null)
			{
				textureToMask.OnAddCommand += HandleAddCommand;
			}

			CwPaintableManager.OnBeginPainting += HandleBeginPainting;
		}

		protected virtual void OnDisable()
		{
			if (textureToMask != null)
			{
				textureToMask.OnAddCommand -= HandleAddCommand;
			}

			CwPaintableManager.OnBeginPainting -= HandleBeginPainting;
		}

		protected virtual void Update()
		{
			CheckMask();

			foreach (var mask in masks)
			{
				mask.Age += Time.deltaTime;

				if (mask.Age > 1.0f && mask.Tex != null)
				{
					mask.Tex = CwHelper.Destroy(mask.Tex);
					mask.Seg = -1;

					mask.Owners.Clear();
				}
			}
		}

		private void CheckMask()
		{
			if (outlineRenderer != null)
			{
				var sprite = outlineRenderer.sprite;

                if (sprite != null)
				{
					var texture = outlineRenderer.sprite.texture;

					if (texture != maskedTexture)
					{
						maskedTexture = texture;

						if (textureToMask != null)
						{
							textureToMask.Width  = texture.width;
							textureToMask.Height = texture.height;
						}

						SegmentBuilder.BuildSegments(texture, outlineThreshold);

						// Build pixels
						segments = new int[texture.width * texture.height];
						width    = texture.width;
						height   = texture.height;

						// Tag with segment IDs
						for (var i = 0; i < SegmentBuilder.GetSegmentCount(); i++)
						{
							var segment = SegmentBuilder.GetSegment(i);

							foreach (var line in segment.Lines)
							{
								var o = line.Y * texture.width;

								for (var x = line.MinX; x < line.MaxX; x++)
								{
									segments[x + o] = i + 1;
								}
							}
						}

						// Feather segment edges
						for (var f = 0; f < feather; f++)
						{
							var t = width * height;

							for (var y = 0; y < height; y++)
							{
								for (var x = 0; x < width; x++)
								{
									var i = x + y * width;
									var s = 0;

									if (segments[i] == 0)
									{
										s = GetSegment(x - 1, y    ); if (s > 0) { segments[i] = -s; continue; }
										s = GetSegment(x + 1, y    ); if (s > 0) { segments[i] = -s; continue; }
										s = GetSegment(x    , y - 1); if (s > 0) { segments[i] = -s; continue; }
										s = GetSegment(x    , y + 1); if (s > 0) { segments[i] = -s; continue; }
									}
								}
							}

							for (var i = 0; i < t; i++)
							{
								segments[i] = System.Math.Abs(segments[i]);
							}
						}

						pixels = new Color32[texture.width * texture.height];
					}
				}
			}
		}

		private void HandleBeginPainting(object owner)
		{
			foreach (var mask in masks)
			{
				if (mask.Owners.Contains(owner) == true)
				{
					mask.Age = 0.0f;
				}
			}
		}

		private void HandleAddCommand(CwCommand command)
		{
			var commandDecal2D = command as CwCommandDecal2D;

			if (commandDecal2D != null)
			{
				if (segments != null && segments.Length > 0 && textureToMask != null && enabled == true)
				{
					if (outlineRenderer != null)
					{
						var p = outlineRenderer.worldToLocalMatrix.MultiplyPoint(commandDecal2D.Position);
						var b = outlineRenderer.localBounds;
						var u = Mathf.RoundToInt(Mathf.InverseLerp(b.min.x, b.max.x, p.x) * width );
						var v = Mathf.RoundToInt(Mathf.InverseLerp(b.min.y, b.max.y, p.y) * height);

						u = Mathf.Clamp(u, 0, width  - 1);
						v = Mathf.Clamp(v, 0, height - 1);

						var s = segments[u + v * width];

						if (s > 0)
						{
							ApplyMask(commandDecal2D, s);
						}
						else
						{
							textureToMask.ClearCommand(commandDecal2D);
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
	using TARGET = ColoringBookMasker;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class ColoringBookMasker_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			BeginError(Any(tgts, t => t.OutlineRenderer == null));
				Draw("outlineRenderer", "The outline is stored in this <b>SpriteRenderer</b>.");
			EndError();
			Draw("outlineThreshold", "Each pixel in the outline must have at least this much alpha opacity for it to be considered part of the outline.");
			BeginError(Any(tgts, t => t.TextureToMask == null));
				Draw("textureToMask", "The masks will be applied to this <b>P3dPaintableTexture</b>.");
			EndError();
			Draw("feather", "How many pixels should the segment borders be extended by? This can remove thin white edges if your outline texture isn't pixel perfect.");
			Draw("use", "How should the coloring book outline be masked?\n\nSegmentCurrentlyUnderFinger = The segment will update based on where your finger is.\n\nSegmentFirstUnderFinger = The segment will remain locked to the first one under your finger when you begin touching the screen.");
		}
	}
}
#endif