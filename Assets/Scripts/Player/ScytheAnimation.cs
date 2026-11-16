using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ScytheAnimation : MonoBehaviour, IPauseable
{
    [Header("Attack Settings")]
    [SerializeField] private float attackDuration = 0.3f;

    private Animator _animator;
    private readonly int _isAttackingHash = Animator.StringToHash("isAttacking");
    private readonly int _attackDirHash = Animator.StringToHash("attackDir"); // 0: Side, 1: Up, 2: Down

    private bool _isAttacking = false;
    private Coroutine _attackCoroutine;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        EventManager.OnSwipeHorizontal += HandleHorizontalSwipe;
        EventManager.OnSwipeUp += HandleSwipeUp;
        EventManager.OnSwipeDown += HandleSwipeDown;
    }

    void OnDisable()
    {
        EventManager.OnSwipeHorizontal -= HandleHorizontalSwipe;
        EventManager.OnSwipeUp -= HandleSwipeUp;
        EventManager.OnSwipeDown -= HandleSwipeDown;
    }

    private void HandleHorizontalSwipe(float direction)
    {
        if (!enabled || !GameManager.IsPlaying) return;
        _animator.SetInteger(_attackDirHash, 0);
        Attack();
    }

    private void HandleSwipeUp()
    {
        if (!enabled || !GameManager.IsPlaying) return;
        _animator.SetInteger(_attackDirHash, 1);
        Attack();
    }

    private void HandleSwipeDown()
    {
        if (!enabled || !GameManager.IsPlaying) return;
        _animator.SetInteger(_attackDirHash, 2);
        Attack();
    }

    private void Attack()
    {
        if (_isAttacking) return;

        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
        }
        _attackCoroutine = StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        _isAttacking = true;
        _animator.SetBool(_isAttackingHash, true);

        float timer = 0f;
        while (timer < attackDuration)
        {
            if (enabled)
            {
                timer += Time.deltaTime;
            }
            yield return null;
        }

        _isAttacking = false;
        _animator.SetBool(_isAttackingHash, false);
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