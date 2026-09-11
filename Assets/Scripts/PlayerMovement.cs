using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Controller _controller;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpForce = 7f;
    private Rigidbody2D _rb;

    private void Awake() => _rb = GetComponent<Rigidbody2D>();
    private void FixedUpdate()
    {
        MoveHorizontal();
        CheckVerticalInput();
    }
    private void MoveHorizontal()
    {
        float horizontalInput = _controller.GetHorizontalInput();
        _rb.linearVelocity = new Vector2(horizontalInput * _speed,_rb.linearVelocity.y);
    }
    private void CheckVerticalInput()
    {
        int verticalInput = _controller.GetVerticalInput();
        if (verticalInput == 0)
            return;
        if (verticalInput > 0)
        {
            JumpUp();
        }
        else
        {
            JumpDown();
        }
    }
    private void JumpUp()
    {
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x,_jumpForce);
    }
    private void JumpDown()
    {
        // Lo implementaremos para atravesar/bajar
        // a la plataforma inferior.
    }
}