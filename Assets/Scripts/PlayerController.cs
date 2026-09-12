using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Configuración de Swipes")]
    public float swipeThreshold = 50f;

    [Header("Capas (Layers)")]
    public string platformLayerName = "Platform";

    [Header("Detección de Suelo")]
    public float groundCheckDistance = 0.2f;

    private Rigidbody2D rb;
    private Collider2D primaryPlayerCollider;
    private int platformLayerMask;

    private Vector2 swipeStartPos;
    private bool isSwiping = false;

    private float moveDirection = 1f;
    private bool isGrounded;
    private Collider2D currentPlatform;
    private bool isDropping = false;
    private Collider2D[] allPlayerColliders;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        primaryPlayerCollider = GetComponent<Collider2D>();
        platformLayerMask = 1 << LayerMask.NameToLayer(platformLayerName);
        allPlayerColliders = GetComponents<Collider2D>();
    }

    void Update()
    {
        CheckGrounded();
        DetectSwipeUnified();
        MoveHorizontally();
    }

    private void CheckGrounded()
    {
        // REGLA 1: Si estamos subiendo (saltando) o ya cayendo, no estamos en el suelo.
        // Esto bloquea que puedas hacer swipe hacia abajo mientras atraviesas la plataforma hacia arriba.
        if (isDropping || rb.linearVelocity.y > 0.1f)
        {
            isGrounded = false;
            currentPlatform = null;
            return;
        }

        Vector2 rayStart = new Vector2(primaryPlayerCollider.bounds.center.x, primaryPlayerCollider.bounds.min.y + 0.05f);
        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, groundCheckDistance, platformLayerMask);

        if (hit.collider != null)
        {
            isGrounded = true;
            currentPlatform = hit.collider;
        }
        else
        {
            isGrounded = false;
            currentPlatform = null;
        }
    }

    private void DetectSwipeUnified()
    {
        if (Input.GetMouseButtonDown(0))
        {
            swipeStartPos = Input.mousePosition;
            isSwiping = true;
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            swipeStartPos = Input.GetTouch(0).position;
            isSwiping = true;
        }

        if (isSwiping && (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)))
        {
            Vector2 swipeEndPos = Input.GetMouseButtonUp(0) ? (Vector2)Input.mousePosition : Input.GetTouch(0).position;
            Vector2 swipeDelta = swipeEndPos - swipeStartPos;
            isSwiping = false;

            if (swipeDelta.magnitude > swipeThreshold)
            {
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                {
                    moveDirection = Mathf.Sign(swipeDelta.x);
                    FlipSprite(moveDirection);
                }
                else
                {
                    if (swipeDelta.y > 0 && isGrounded)
                    {
                        Jump();
                    }
                    else if (swipeDelta.y < 0 && isGrounded && !isDropping)
                    {
                        StartCoroutine(DropThroughPlatform());
                    }
                }
            }
        }
    }

    private void MoveHorizontally()
    {
        rb.linearVelocity = new Vector2(moveSpeed * moveDirection, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private IEnumerator DropThroughPlatform()
    {
        if (currentPlatform != null)
        {
            isDropping = true;
            isGrounded = false;
            Collider2D platformToDrop = currentPlatform;

            foreach (Collider2D playerCollider in allPlayerColliders)
            {
                Physics2D.IgnoreCollision(playerCollider, platformToDrop, true);
            }

            // Un empujón más fuerte para garantizar que penetre la plataforma
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -5f);

            // REGLA 2: Esperamos una fracción de segundo OBLIGATORIA para que el personaje 
            // se hunda en la plataforma antes de empezar a medir si ya salió.
            yield return new WaitForSeconds(0.15f);

            float timer = 0f;
            while (primaryPlayerCollider.bounds.Intersects(platformToDrop.bounds) && timer < 0.5f)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            foreach (Collider2D playerCollider in allPlayerColliders)
            {
                if (playerCollider != null)
                    Physics2D.IgnoreCollision(playerCollider, platformToDrop, false);
            }

            isDropping = false;
        }
    }

    private void FlipSprite(float direction)
    {
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Abs(localScale.x) * direction;
        transform.localScale = localScale;
    }
}