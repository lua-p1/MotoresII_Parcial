using UnityEngine;
public class PlatformDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _platformLayer;
    [SerializeField] private float _checkDistance = 2f;
    public bool TryGetPlatformAbove(out IPlatform platform) => TryGetPlatform(Vector2.up, out platform);
    public bool TryGetPlatformBelow(out IPlatform platform) => TryGetPlatform(Vector2.down, out platform);
    private bool TryGetPlatform(Vector2 direction, out IPlatform platform)
    {
        platform = null;
        RaycastHit2D hit = Physics2D.Raycast(transform.position,direction,_checkDistance,_platformLayer);
        if (hit.collider == null)
            return false;
        platform = hit.collider.GetComponent<IPlatform>();
        return platform != null;
    }
}
