using UnityEngine;
using UnityEngine.EventSystems;

namespace CustomUIComponents.Tooltips
{
    public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string title = "Tooltip Title";
        public string description = "Tooltip Description";
        
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
