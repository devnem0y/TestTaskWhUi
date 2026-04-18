using UnityEngine;
using UnityEngine.UI;

public class PartySizeController : MonoBehaviour
{
    [SerializeField] private LayoutElement _layoutElement;
    [SerializeField] private RectTransform _rectContent;
    
    public void Rebuild()
    {
        _layoutElement.minWidth = _rectContent.rect.width - 20f;
    }
}