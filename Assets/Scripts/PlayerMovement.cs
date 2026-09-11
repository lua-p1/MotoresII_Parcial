using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Controller _controller;
    [SerializeField] float _speed;
    private Rigidbody2D _rb;
    private void Awake() => _rb = GetComponent<Rigidbody2D>();
    private void FixedUpdate()
    {
        Vector2 input = _controller.GetMovementInput();
        MoveHorizontal(input);
    }

    private void MoveHorizontal(Vector2 input)
    {
       _rb.linearVelocity = new Vector2(input.x * _speed, _rb.linearVelocity.y);
    }
}
