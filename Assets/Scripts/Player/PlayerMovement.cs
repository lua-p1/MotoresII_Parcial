using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(GroundDetector))]
[RequireComponent(typeof(WallDetector))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Config")]
    public float moveSpeed = 7f;
    public float jumpForce = 20f;
    [Range(0f, 1f)]
    public float airSpeedMultiplier = 0.35f;

    [Header("Gravity Config")]
    public float fallMultiplier = 2.5f;

    private Rigidbody2D _rb;
    private PlayerInput _input;
    private GroundDetector _groundDetector;
    private WallDetector _wallDetector;
    private Collider2D _playerCollider;
    private float _moveDirection = 1f;
    private float _defaultGravity;
    private bool _isStopped = true;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _groundDetector = GetComponent<GroundDetector>();
        _wallDetector = GetComponent<WallDetector>();
        _playerCollider = GetComponent<Collider2D>();
        _defaultGravity = _rb.gravityScale;
    }
    void OnEnable()
    {
        EventManager.OnSwipeHorizontal += HandleHorizontalSwipe;
        EventManager.OnSwipeUp += Jump;
        EventManager.OnSwipeDown += AttemptDrop;
        EventManager.OnGameOver += HandleGameOver;
    }
    void OnDisable()
    {
        EventManager.OnSwipeHorizontal -= HandleHorizontalSwipe;
        EventManager.OnSwipeUp -= Jump;
        EventManager.OnSwipeDown -= AttemptDrop;
        EventManager.OnGameOver -= HandleGameOver;
    }
    void Update()
    {
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing)
        {
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y); // Frenamos en seco en X
            return;
        }
        CheckWallCollision();
        MoveHorizontally();
        ApplyBetterJump();
    }
    private void HandleGameOver()
    {
        _isStopped = true;
        _rb.linearVelocity = Vector2.zero;
    }
    private void HandleHorizontalSwipe(float direction)
    {
        _isStopped = false;
        ChangeDirection(direction);
    }
    private void CheckWallCollision()
    {
        if (_wallDetector.IsTouchingWall && !_isStopped)
        {
            ChangeDirection(-_moveDirection);
            _isStopped = true;
        }
    }
    private void MoveHorizontally()
    {
        if (_isStopped)
        {
            _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);
            return;
        }
        if (_groundDetector.IsGrounded || _groundDetector.IsDropping || _rb.linearVelocity.y < -0.1f)
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
        if (direction == 0) return;
        _moveDirection = Mathf.Sign(direction);
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Abs(localScale.x) * _moveDirection;
        transform.localScale = localScale;
    }
    private void Jump()
    {
        if (_groundDetector.IsGrounded)
        {
            _isStopped = false;
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0);
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    private void AttemptDrop()
    {
        if (_groundDetector.IsGrounded && !_groundDetector.IsDropping)
        {
            _isStopped = false;
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