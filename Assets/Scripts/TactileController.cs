using UnityEngine;

public class TactileController : Controller
{
    [SerializeField] float _deadZone = 50f;
    private Vector2 _startTouchPos;
    public override Vector2 GetMovementInput() => _moveDir;

    public void StartTouch(Vector2 position) => _startTouchPos = position;
    public void DragTouch(Vector2 position)
    {
        Vector2 drag = position - _startTouchPos;
        if (drag.magnitude < _deadZone)
        {
            NotMove();
            return;
        }
        if (Mathf.Abs(drag.x) < Mathf.Abs(drag.y))
        {
            _moveDir = drag.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            _moveDir = drag.y > 0 ? Vector2.up : Vector2.down;
        }
    }
    public void EndTouch() => NotMove();
}
