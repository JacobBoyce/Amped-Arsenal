using UnityEngine;
using UnityEngine.UI;

public class UIBGScroller : MonoBehaviour
{
    [SerializeField] private RawImage _img;
    [SerializeField] private float _x, _y;

    private void Update()
    {
        _img.uvRect = new Rect(_img.uvRect.position + new Vector2(_x, _y) * Time.unscaledDeltaTime, _img.uvRect.size);
    }
}
