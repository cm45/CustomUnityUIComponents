using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CustomUIComponents.Tooltips
{
	[RequireComponent(typeof(CanvasGroup))]
	public class TooltipController : MonoBehaviour
	{
		public Vector2 offset = Vector2.up * 64;
		public TooltipOffsetMode offsetMode;

		[Header("UI References")] [SerializeField]
		private TextMeshProUGUI titleText;

		[SerializeField] private TextMeshProUGUI descriptionText;
		[SerializeField] private Image iconImage;

		private CanvasGroup canvasGroup;
		private Tooltip currentTooltip;

		private static TooltipController _instance;

		private void Awake()
		{
			_instance = this;
			canvasGroup = GetComponent<CanvasGroup>();
			HideTooltip();
		}

		private void Update()
		{
			if (currentTooltip == null)
				return;

			var forceMousePosition = offsetMode == TooltipOffsetMode.ForceMousePosition || (offsetMode == TooltipOffsetMode.Default && currentTooltip.tooltipOffsetMode == TooltipOffsetMode.ForceMousePosition);

			if (forceMousePosition)
				transform.position = Input.mousePosition + (Vector3) offset;
		}

		private void SetTooltip(Tooltip tooltip)
		{
			currentTooltip = tooltip;

			// TODO: Null-Checks?
			titleText.text = tooltip.title;
			descriptionText.text = tooltip.description;

			if (iconImage)
				iconImage.sprite = tooltip.icon;
		}

		public static void ShowTooltip(Tooltip tooltip)
		{
			_instance.SetTooltip(tooltip);
			_instance.transform.position = tooltip.transform.position + (Vector3) _instance.offset;
			_instance.canvasGroup.alpha = 1;
		}

		public static void HideTooltip()
		{
			_instance.canvasGroup.alpha = 0;
		}
	}

	public enum TooltipOffsetMode
	{
		Default,
		ForceMousePosition,
		ForceObjectPosition
	}
}