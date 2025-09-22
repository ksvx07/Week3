using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea] public string tooltipText; // Inspector에 설명 적기
    public Tooltip tooltip; // Tooltip UI 참조

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Show(tooltipText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Hide();
    }
}
