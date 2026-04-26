using UnityEngine;
using UnityEngine.UI;

public static class CustomSizeUtils
{
    public static void RebuildWidth(LayoutElement layoutElement, RectTransform rectTransform, float offset = 0f)
    {
        layoutElement.minWidth = rectTransform.rect.width - offset;
    }
}