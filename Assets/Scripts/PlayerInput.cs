using System;
using UnityEngine;
public class PlayerInput : MonoBehaviour
{
    [Header("Swipe Config")]
    public float swipeThreshold = 50f;

    public event Action<float> OnSwipeHorizontal;
    public event Action OnSwipeUp;
    public event Action OnSwipeDown;
    private Vector2 _swipeStartPos;
    private bool _isSwiping = false;
    void Update()
    {
        DetectSwipe();
    }
    private void DetectSwipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _swipeStartPos = Input.mousePosition;
            _isSwiping = true;
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            _swipeStartPos = Input.GetTouch(0).position;
            _isSwiping = true;
        }
        if (_isSwiping && (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)))
        {
            Vector2 swipeEndPos = Input.GetMouseButtonUp(0) ? (Vector2)Input.mousePosition : Input.GetTouch(0).position;
            Vector2 swipeDelta = swipeEndPos - _swipeStartPos;
            _isSwiping = false;
            if (swipeDelta.magnitude > swipeThreshold)
            {
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                {
                    OnSwipeHorizontal?.Invoke(Mathf.Sign(swipeDelta.x));
                }
                else
                {
                    if (swipeDelta.y > 0) OnSwipeUp?.Invoke();
                    else if (swipeDelta.y < 0) OnSwipeDown?.Invoke();
                }
            }
        }
    }
}