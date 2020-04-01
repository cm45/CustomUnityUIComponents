using UnityEngine;
using UnityEngine.EventSystems;

namespace CustomUIComponents.Tooltips
{
    public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private string title = "Tooltip Title";
        [SerializeField] private string description = "Tooltip Description.";
        
        // Inherit Tooltip class & Override properties to customize tooltip
        public virtual string Title
        {
            get => title;
            set => title = value;
        }

        public virtual string Description
        {
            get => description;
            set => description = value;
        }

        public Sprite icon;

        public TooltipOffsetMode tooltipOffsetMode = TooltipOffsetMode.Default;

        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipController.ShowTooltip(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipController.HideTooltip();
        }
    }
}
