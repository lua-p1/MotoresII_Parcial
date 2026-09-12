using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(GroundDetector))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float moveSpeed = 7f;
    public float jumpForce = 20f;
    [Range(0f, 1f)]
    public float airSpeedMultiplier = 0.35f;

    [Header("Gravity Config")]
    public float fallMultiplier = 2.5f;

    private Rigidbody2D _rb;
    private PlayerInput _input;
    private GroundDetector _groundDetector;
    private Collider2D _playerCollider;
    private float _moveDirection = 1f;
    private float _defaultGravity;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _groundDetector = GetComponent<GroundDetector>();
        _playerCollider = GetComponent<Collider2D>();
        _defaultGravity = _rb.gravityScale;
    }
    void OnEnable()
    {
        _input.OnSwipeHorizontal += ChangeDirection;
        _input.OnSwipeUp += Jump;
        _input.OnSwipeDown += AttemptDrop;
    }
    void OnDisable()
    {
        _input.OnSwipeHorizontal -= ChangeDirection;
        _input.OnSwipeUp -= Jump;
        _input.OnSwipeDown -= AttemptDrop;
    }
    void Update()
    {
        MoveHorizontally();
        ApplyBetterJump();
    }
    private void MoveHorizontally()
    {
        if (_groundDetector.IsGrounded)
        {
            _rb.linearVelocity = new Vector2(moveSpeed * _moveDirection, _rb.linearVelocity.y);
        }
        else
        {
            float airSpeed = moveSpeed * airSpeedMultiplier;
            _rb.linearVelocity = new Vector2(airSpeed * _moveDirection, _rb.linearVelocity.y);
        }
    }
    private void ApplyBetterJump()
    {
        if (_rb.linearVelocity.y < 0)
        {
            _rb.gravityScale = _defaultGravity * fallMultiplier;
        }
        else
        {
            _rb.gravityScale = _defaultGravity;
        }
    }
    private void ChangeDirection(float direction)
    {
        _moveDirection = direction;
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Abs(localScale.x) * direction;
        transform.localScale = localScale;
    }
    private void Jump()
    {
        if (_groundDetector.IsGrounded)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0);
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    private void AttemptDrop()
    {
        if (_groundDetector.IsGrounded && !_groundDetector.IsDropping)
        {
            StartCoroutine(DropThroughPlatformRoutine());
        }
    }
    private IEnumerator DropThroughPlatformRoutine()
    {
        if (_groundDetector.CurrentPlatform != null)
        {
            _groundDetector.IsDropping = true;
            Collider2D platformToDrop = _groundDetector.CurrentPlatform;
            Physics2D.IgnoreCollision(_playerCollider, platformToDrop, true);
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, -5f);
            yield return new WaitForSeconds(0.15f);
            float timer = 0f;
            while (_playerCollider.bounds.Intersects(platformToDrop.bounds) && timer < 0.5f)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            if (_playerCollider != null)
            {
                Physics2D.IgnoreCollision(_playerCollider, platformToDrop, false);
            }
            _groundDetector.IsDropping = false;
        }
    }
}