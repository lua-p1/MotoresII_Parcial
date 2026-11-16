using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(GroundDetector))]
public class PlayerAnimation : MonoBehaviour, IPauseable
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private GroundDetector _groundDetector;

    private readonly int _isStillHash = Animator.StringToHash("isStill");
    private readonly int _isWalkingHash = Animator.StringToHash("isWalking");
    private readonly int _isJumpingHash = Animator.StringToHash("isJumping");

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _groundDetector = GetComponent<GroundDetector>();
    }

    void Update()
    {
        if (!GameManager.IsPlaying)
        {
            _animator.SetBool(_isStillHash, true);
            _animator.SetBool(_isWalkingHash, false);
            _animator.SetBool(_isJumpingHash, false);
            return;
        }

        UpdateAnimations();
    }

    private void UpdateAnimations()
    {
        bool isGrounded = _groundDetector.IsGrounded;
        float horizontalSpeed = Mathf.Abs(_rb.linearVelocity.x);

        bool isJumping = !isGrounded;
        bool isWalking = isGrounded && horizontalSpeed > 0.1f;
        bool isStill = isGrounded && horizontalSpeed <= 0.1f;

        _animator.SetBool(_isJumpingHash, isJumping);
        _animator.SetBool(_isWalkingHash, isWalking);
        _animator.SetBool(_isStillHash, isStill);
    }

    public void Pause()
    {
        if (_animator != null) _animator.speed = 0f;
        enabled = false;
    }

    public void Resume()
    {
        if (_animator != null) _animator.speed = 1f;
        enabled = true;
    }
}