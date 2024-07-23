using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CW.Common;

namespace PaintIn2D.ColoringBook
{
	/// <summary>This component can be used to make painting tools that you can click/tap to select.
	/// When selected, they will slide in to view, and the specified <b>ToolRoot</b> GameObject will be activated.</summary>
	[ExecuteInEditMode]
	public class ToolButton : MonoBehaviour, IPointerDownHandler
	{
		/// <summary>The color of this tool.</summary>
		public Color Color { set { color = value; UpdateColor(); } get { return color; } } [SerializeField] private Color color;

		/// <summary>Automatically select this tool in <b>Start</b>?</summary>
		public bool SelectByDefault { set { selectByDefault = value; } get { return selectByDefault; } } [SerializeField] private bool selectByDefault;

		/// <summary>Automatically change the Color based on the last selected color?</summary>
		public bool UseColorButtonColor { set { useColorButtonColor = value; } get { return useColorButtonColor; } } [SerializeField] private bool useColorButtonColor;

		/// <summary>The current color will be applied to this UI Image.</summary>
		public Image ColorVisual { set { colorVisual = value; } get { return colorVisual; } } [SerializeField] private Image colorVisual;

		/// <summary>The current color will be applied to this paint tool.</summary>
		public CwPaintDecal2D ColorTool { set { colorTool = value; } get { return colorTool; } } [SerializeField] private CwPaintDecal2D colorTool;

		/// <summary>The <b>RectTransform</b> that will be slid based on the selection state.</summary>
		public RectTransform VisualRoot { set { visualRoot = value; } get { return visualRoot; } } [SerializeField] private RectTransform visualRoot;

		/// <summary>This paint brush tool will be de/activated based on this color's selection state.</summary>
		public GameObject ToolRoot { set { toolRoot = value; } get { return toolRoot; } } [SerializeField] private GameObject toolRoot;

		/// <summary>The <b>VisualRoot</b> component's <b>AnchoredPosition</b> value when not selected.</summary>
		public Vector2 DefaultAnchoredPosition { set { defaultAnchoredPosition = value; } get { return defaultAnchoredPosition; } } [SerializeField] private Vector2 defaultAnchoredPosition = new Vector2(100.0f, 0.0f);

		/// <summary>The <b>VisualRoot</b> component's <b>AnchoredPosition</b> value when this is selected.</summary>
		public Vector2 SelectedAnchoredPosition { set { selectedAnchoredPosition = value; } get { return selectedAnchoredPosition; } } [SerializeField] private Vector2 selectedAnchoredPosition = new Vector2(0.0f, 0.0f);

		/// <summary>How quick the sliding animation is.</summary>
		public float Damping { set { damping = value; } get { return damping; } } [SerializeField] private float damping = 10.0f;

		private static ToolButton current;

		public void UpdateColor()
		{
			var c = color; c.a = 1.0f;

			if (colorVisual != null)
			{
				colorVisual.color = c;
			}

			if (colorTool != null)
			{
				colorTool.Color = c;
			}
		}

		public void DoSelect()
		{
			current = this;
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
			var targetAP = defaultAnchoredPosition;

			if (current == this)
			{
				toolRoot.SetActive(true);

				targetAP = selectedAnchoredPosition;
			}
			else
			{
				toolRoot.SetActive(false);
			}

			var factor = CwHelper.DampenFactor(damping, Time.deltaTime);

			visualRoot.anchoredPosition = Vector2.Lerp(visualRoot.anchoredPosition, targetAP, factor);

			if (useColorButtonColor == true && ColorButton.CurrentColorButton != null)
			{
				color = ColorButton.CurrentColorButton.Color;
			}

			UpdateColor();
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn2D.ColoringBook
{
	using CW.Common;
	using UnityEditor;
	using TARGET = ToolButton;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class SlidingToolButton_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("color", "The color of this tool.");
			Draw("selectByDefault", "This many prefabs will be spawned, and be given these colors.");
			Draw("useColorButtonColor", "Automatically change the Color based on the last selected color?");

			Separator();

			Draw("colorVisual", "The current color will be applied to this UI Image.");
			Draw("colorTool", "The current color will be applied to this paint tool.");
			BeginError(Any(tgts, t => t.VisualRoot == null));
				Draw("visualRoot", "The <b>RectTransform</b> that will be slid based on the selection state.");
			EndError();
			BeginError(Any(tgts, t => t.ToolRoot == null));
				Draw("toolRoot", "This paint brush tool will be de/activated based on this color's selection state.");
			EndError();

			Separator();

			Draw("defaultAnchoredPosition", "The <b>VisualRoot</b> component's <b>AnchoredPosition</b> value when not selected.");
			Draw("selectedAnchoredPosition", "The <b>VisualRoot</b> component's <b>AnchoredPosition</b> value when this is selected.");
			Draw("damping", "How quick the sliding animation is.");
		}
	}
}
#endif