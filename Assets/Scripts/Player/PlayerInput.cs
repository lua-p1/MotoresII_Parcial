using UnityEngine;
public class PlayerInput : MonoBehaviour
{
    [Header("Swipe Config")]
    public float swipeThreshold = 20f;
    private Vector2 _swipeStartPos;
    private bool _isSwiping = false;
    void Update()
    {
        if (GameManager.Instance != null &&
            (GameManager.Instance.CurrentState == GameManager.GameState.GameOver ||
             GameManager.Instance.CurrentState == GameManager.GameState.Victory))
            return;
        DetectSwipe();
    }
    private void DetectSwipe()
    {
        bool mouseDown = false;
        bool mouseUp = false;
#if UNITY_EDITOR
        mouseDown = Input.GetMouseButtonDown(0);
        mouseUp = Input.GetMouseButtonUp(0);   
#endif
        if (mouseDown)
        {
            _swipeStartPos = Input.mousePosition;
            _isSwiping = true;
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            _swipeStartPos = Input.GetTouch(0).position;
            _isSwiping = true;
        }
        if (_isSwiping && (mouseUp || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)))
        {
            Vector2 swipeEndPos = mouseUp ? (Vector2)Input.mousePosition : Input.GetTouch(0).position;
            Vector2 swipeDelta = swipeEndPos - _swipeStartPos;
            _isSwiping = false;
            if (swipeDelta.magnitude > swipeThreshold)
            {
                if (GameManager.Instance.CurrentState == GameManager.GameState.WaitingToStart)
                {
                    GameManager.Instance.StartGame();
                }
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                {
                    EventManager.OnSwipeHorizontal?.Invoke(Mathf.Sign(swipeDelta.x));
                }
                else
                {
                    if (swipeDelta.y > 0) EventManager.OnSwipeUp?.Invoke();
                    else if (swipeDelta.y < 0) EventManager.OnSwipeDown?.Invoke();
                }
            }
        }
    }
}