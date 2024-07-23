using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PaintIn2D.ColoringBook
{
	/// <summary>This component allows you to make a button that can change the color of brushes in the scene.</summary>
	[ExecuteInEditMode]
	public class ColorButton : MonoBehaviour, IPointerDownHandler
	{
		/// <summary>The color of this button.</summary>
		public Color Color { set { color = value; UpdateColor(); } get { return color; } } [SerializeField] private Color color;

		/// <summary>Automatically select this color in <b>Start</b>?</summary>
		public bool SelectByDefault { set { selectByDefault = value; } get { return selectByDefault; } } [SerializeField] private bool selectByDefault;

		/// <summary>This image will have its sprite changed based on this color's selection state.</summary>
		public Image ColorVisual { set { colorVisual = value; UpdateSprite(); } get { return colorVisual; } } [SerializeField] private Image colorVisual;

		/// <summary>This sprite will be used when this color isn't selected.</summary>
		public Sprite DefaultSprite { set { defaultSprite = value; UpdateSprite(); } get { return defaultSprite; } } [SerializeField] private Sprite defaultSprite;

		/// <summary>This sprite will be used when this color is selected.</summary>
		public Sprite PickedSprite { set { pickedSprite = value; } get { return pickedSprite; } } [SerializeField] private Sprite pickedSprite;

		private static ColorButton currentColorButton;

		public static ColorButton CurrentColorButton
		{
			get
			{
				return currentColorButton;
			}
		}

		public void UpdateSprite()
		{
			if (colorVisual != null)
			{
				if (currentColorButton == this)
				{
					colorVisual.sprite = pickedSprite;
				}
				else
				{
					colorVisual.sprite = defaultSprite;
				}
			}
		}

		public void UpdateColor()
		{
			var c = color; c.a = 1.0f;

			if (colorVisual != null)
			{
				colorVisual.color = c;
			}
		}

		public void DoSelect()
		{
			currentColorButton = this;

			foreach (var cba in ColorButtonAction.Instances)
			{
				cba.OnNewColor.Invoke(color);
			}
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			DoSelect();
		}

		protected virtual void Start()
		{
			if (selectByDefault == true)
			{
				DoSelect();
			}
		}

		protected virtual void Update()
		{
			UpdateSprite();
			UpdateColor();
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn2D.ColoringBook
{
	using CW.Common;
	using UnityEditor;
	using TARGET = ColorButton;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class ColorButton_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("color", "The color of this button.");
			Draw("selectByDefault", "Automatically select this color in <b>Start</b>?");

			Separator();

			BeginError(Any(tgts, t => t.ColorVisual == null));
				Draw("colorVisual", "This image will have its sprite changed based on this color's selection state.");
			EndError();
			BeginError(Any(tgts, t => t.DefaultSprite == null));
				Draw("defaultSprite", "This sprite will be used when this color isn't selected.");
			EndError();
			BeginError(Any(tgts, t => t.PickedSprite == null));
				Draw("pickedSprite", "This sprite will be used when this color is selected.");
			EndError();
		}
	}
}
#endif