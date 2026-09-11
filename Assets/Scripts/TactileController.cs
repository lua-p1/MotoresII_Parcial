using UnityEngine;
using UnityEngine.EventSystems;
public class TactileController : Controller, IBeginDragHandler, IDragHandler
{
    [SerializeField] private float _dragThreshold = 50f;
    private Vector2 _dragStartPosition;
    public void OnBeginDrag(PointerEventData eventData) => _dragStartPosition = eventData.position;
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 drag = eventData.position - _dragStartPosition;
        if (drag.magnitude < _dragThreshold)
            return;
        if (Mathf.Abs(drag.x) > Mathf.Abs(drag.y))
        {
            if (drag.x > 0)
                MoveRight();
            else
                MoveLeft();
        }
        else
        {
            if (drag.y > 0)
                MoveUp();
            else
                MoveDown();
        }
        _dragStartPosition = eventData.position;
    }
    public void MoveRight() => _horizontalInput = 1f;
    public void MoveLeft() =>_horizontalInput = -1f;
    public void MoveUp() => _verticalInput = 1;
    public void MoveDown() =>_verticalInput = -1;

}
