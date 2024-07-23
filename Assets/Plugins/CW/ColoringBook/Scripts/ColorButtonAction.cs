using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PaintIn2D.ColoringBook
{
	/// <summary>This component invokes an event with a color when you click/tap on a <b>ColorButton</b> component.
	/// This allows you to change the brush color in-game.</summary>
	public class ColorButtonAction : MonoBehaviour
	{
		[System.Serializable] public class ColorEvent : UnityEvent<Color> {}

		/// <summary>This event is invoked when a <b>ColorButton</b> component is clicked/tapped.
		/// Color = The color that was selected.</summary>
		public ColorEvent OnNewColor { get { if (onNewColor == null) onNewColor = new ColorEvent(); return onNewColor; } } [SerializeField] private ColorEvent onNewColor;

		public static LinkedList<ColorButtonAction> Instances = new LinkedList<ColorButtonAction>();

		private LinkedListNode<ColorButtonAction> node;

		protected virtual void OnEnable()
		{
			node = Instances.AddLast(this);
		}

		protected virtual void OnDisable()
		{
			Instances.Remove(node);

			node = null;
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn2D.ColoringBook
{
	using CW.Common;
	using UnityEditor;
	using TARGET = ColorButtonAction;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class ColorButtonAction_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("onNewColor", "This event is invoked when a <b>ColorButton</b> component is clicked/tapped.\n\nColor = The color that was selected.");
		}
	}
}
#endif