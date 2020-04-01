using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CustomUIComponents.Tooltips
{
	[RequireComponent(typeof(CanvasGroup))]
	public class TooltipController : MonoBehaviour
	{
		[Header("Offset")] public Vector2 offset = Vector2.one * 16;
		public TooltipOffsetMode offsetMode;
		public TooltipPositionAnchor preferredAnchor = TooltipPositionAnchor.Vertical;
		public bool allowOverflow = false;

		[Header("UI References")] [SerializeField]
		private TextMeshProUGUI titleText;

		[SerializeField] private TextMeshProUGUI descriptionText;
		[SerializeField] private Image iconImage;

		private CanvasGroup canvasGroup;
		private Tooltip currentTooltip;

		private Canvas canvas;

		private static TooltipController _instance;

		private void Awake()
		{
			if (_instance != null)
			{
				Debug.LogError("Only one instance of 'Tooltip Controller' allowed!"); // TODO: Per scene limit?! or don't destroy on load (bool toggle?)
				return;
			}

			_instance = this;

			canvasGroup = GetComponent<CanvasGroup>();
			canvas = GetComponentInParent<Canvas>();

			HideTooltip();
		}

		private void Update()
		{
			if (currentTooltip == null)
				return;

			var forceMousePosition = offsetMode == TooltipOffsetMode.ForceMousePosition || (offsetMode == TooltipOffsetMode.Default && currentTooltip.tooltipOffsetMode == TooltipOffsetMode.ForceMousePosition);

			if (forceMousePosition)
				transform.position = Input.mousePosition + CalculateOffset(currentTooltip);
		}

		private void SetTooltip(Tooltip tooltip)
		{
			currentTooltip = tooltip;

			// TODO: Null-Checks?
			titleText.text = tooltip.Title;
			descriptionText.text = tooltip.Description;

			if (iconImage)
				iconImage.sprite = tooltip.icon;
		}

		private void RefreshCanvas()
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
			Canvas.ForceUpdateCanvases();
		}

		private Vector3 CalculateOffset(Tooltip tooltip)
		{
			RefreshCanvas();
			
			Vector2 size;

			// Get Object size
			var rectTransform = tooltip.gameObject.GetComponent<RectTransform>();
			if (rectTransform)
				size = rectTransform.sizeDelta;
			else
				size = tooltip.transform.localScale; // TODO: !

			var xOffset = offset.x;
			var yOffset = offset.y;

			var controllerRectTransform = GetComponent<RectTransform>();

			if (preferredAnchor == TooltipPositionAnchor.Horizontal) xOffset += (size.x + controllerRectTransform.rect.width) / 2 * Mathf.Sign(xOffset);
			if (preferredAnchor == TooltipPositionAnchor.Vertical) yOffset += (size.y + controllerRectTransform.rect.height) / 2 * Mathf.Sign(yOffset);

			RefreshCanvas();
			
			return new Vector3(xOffset, yOffset);
		}

		private void MoveTooltip(Tooltip tooltip)
		{
			var newOffset = CalculateOffset(tooltip);
			transform.position = tooltip.transform.position + newOffset;
			
			RefreshCanvas();
			
			// Check for Overflow TODO: Add calculation before movement (in CalculateOffset())
			if (!allowOverflow)
			{
				var controllerRectTransform = transform.GetComponent<RectTransform>();
				
				var x = controllerRectTransform.anchoredPosition.x + controllerRectTransform.rect.width / 2;
				var y = controllerRectTransform.anchoredPosition.y + controllerRectTransform.rect.height / 2;

				var canvasRect = canvas.GetComponent<RectTransform>().rect;
				
				if (x > canvasRect.width / 2)
					transform.position = tooltip.transform.position - new Vector3(newOffset.x, -newOffset.y);
				if (y > canvasRect.height / 2)
					transform.position = tooltip.transform.position - new Vector3(-newOffset.x, newOffset.y);
			}
		}

		public static void ShowTooltip(Tooltip tooltip)
		{
			_instance.SetTooltip(tooltip);
			_instance.MoveTooltip(tooltip);
			_instance.canvasGroup.alpha = 1;
		}

		public static void HideTooltip()
		{
			_instance.canvasGroup.alpha = 0;
		}
	}

	public enum TooltipPositionAnchor
	{
		Vertical,
		Horizontal,
		Center
	}

	public enum TooltipOffsetMode
	{
		Default,
		ForceMousePosition,
		ForceObjectPosition
	}
}