using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class GroundDetector : MonoBehaviour
{
    [Header("Ground Detector")]
    public float groundCheckDistance = 0.2f;
    public string platformLayerName = "Platform";

    public bool IsGrounded { get; private set; }
    public Collider2D CurrentPlatform { get; private set; }
    public bool IsDropping { get; set; }
    private Collider2D _primaryCollider;
    private Rigidbody2D _rb;
    private int _platformLayerMask;
    void Start()
    {
        _primaryCollider = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _platformLayerMask = 1 << LayerMask.NameToLayer(platformLayerName);
    }
    void Update()
    {
        CheckGrounded();
    }
    private void CheckGrounded()
    {
        if (IsDropping || _rb.linearVelocity.y > 0.1f)
        {
            IsGrounded = false;
            CurrentPlatform = null;
            return;
        }
        Vector2 rayStart = new Vector2(_primaryCollider.bounds.center.x, _primaryCollider.bounds.min.y + 0.05f);
        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, groundCheckDistance, _platformLayerMask);
        if (hit.collider != null)
        {
            IsGrounded = true;
            CurrentPlatform = hit.collider;
        }
        else
        {
            IsGrounded = false;
            CurrentPlatform = null;
        }
    }
}