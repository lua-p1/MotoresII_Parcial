using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class WallDetector : MonoBehaviour
{
    [Header("Wall Detector")]
    public float wallCheckDistance = 0.1f;
    public string wallLayerName = "Wall";

    public bool IsTouchingWall { get; private set; }
    private Collider2D _primaryCollider;
    private int _wallLayerMask;
    void Start()
    {
        _primaryCollider = GetComponent<Collider2D>();
        _wallLayerMask = 1 << LayerMask.NameToLayer(wallLayerName);
    }
    void Update()
    {
        CheckWall();
    }
    private void CheckWall()
    {
        float facingDirection = Mathf.Sign(transform.localScale.x);
        Vector2 rayDirection = new Vector2(facingDirection, 0);
        Vector2 rayStart = new Vector2(_primaryCollider.bounds.center.x + (facingDirection * _primaryCollider.bounds.extents.x),_primaryCollider.bounds.center.y);
        RaycastHit2D hit = Physics2D.Raycast(rayStart, rayDirection, wallCheckDistance, _wallLayerMask);
        IsTouchingWall = hit.collider != null;
    }
}