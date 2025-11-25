using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Witch : MonoBehaviour
{
    [Header("state")]
    IWitchState _currentState;
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _jumpForce = 5f;
    bool _jumpInputPressed;
    bool _interactInputPressed;
    Vector2 _moveInput;
    IInteractable _currentInteractable;

    [Header("땅체크")]
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] Transform _groundChecker;
    [SerializeField] float _groundCheckDistance = 0.1f;
    bool _isGrounded;

    [Header("피직스마테리얼")]
    public PhysicsMaterial2D _noFriction;
    public PhysicsMaterial2D _useFriction;

    [Header("MVC")]
    private int _currentHp;
    private int _maxHp=10;
    public event Action<int,int> OnHpChanged;

    [Header("넉백설정")]
    [SerializeField] private float _knockbackForce = 5f;
    [SerializeField] private float _hitDuration = 0.2f;
 
    Rigidbody2D _rb;
    SpriteRenderer _spr;
    Animator _animator;

    //프로퍼티
    public int CurrentHp => _currentHp;
    public int MaxHp => _maxHp;
    public float KnockbackForce => _knockbackForce;
    public float HitDuration => _hitDuration;
    public float MoveSpeed => _moveSpeed;
    public float JumpForce => _jumpForce;
    public Vector2 MoveInput => _moveInput;
    public Rigidbody2D Rb => _rb;
    public SpriteRenderer Spr => _spr;
    public bool JumpInputPressed => _jumpInputPressed;
    public bool IsGrounded => _isGrounded;
    public bool InteractInputPressed => _interactInputPressed;
    public Animator Animator => _animator;

    private void Awake()
    {
        _currentHp = _maxHp;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _spr = GetComponent<SpriteRenderer>();
        OnHpChanged?.Invoke(_currentHp, _maxHp);
        SetState(new IdleState(this));
    }

    private void Update()
    {
        _isGrounded = Physics2D.Raycast(
            _groundChecker.position,    //발사위치
            Vector2.down,               //발사방향
            _groundCheckDistance,       //레이저길이
            _groundLayer                //충돌대상체크
            );
        _currentState.Update();
        _jumpInputPressed = false;
        _interactInputPressed = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if(interactable != null)
        {
            _currentInteractable = interactable;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        // 나가는 오브젝트가 현재 상호작용 가능 아이템과 같다면 참조를 해제합니다.
        if (other.GetComponent<IInteractable>() == _currentInteractable)
        {
            _currentInteractable = null;
        }
    }

    public void SetState(IWitchState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
    public void OnMove(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            _jumpInputPressed = true;
        }
    }
    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            _interactInputPressed = true;
        }
    }
    public void TryInteract()
    {
        if(_currentInteractable != null)
        {
            _currentInteractable.Interact(this);
            
        }
    }
    public void Die()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.RespawnPlayer(this);
        }
    }

    public void Respawn()
    {
        _currentHp = _maxHp;
        OnHpChanged?.Invoke(_currentHp, _maxHp);
        _rb.linearVelocity = Vector2.zero;

    }
    public void TakeDamager(int damage,Vector3 hitPos)
    {
        if (_currentHp <= 0)
        {
            _currentHp = 0;
            Die();
            return;
        }

        SetState(new HitState(this, hitPos));

        _currentHp -= damage;
        _currentHp = Mathf.Max(0, _currentHp);
        OnHpChanged?.Invoke(_currentHp, _maxHp);
    }
}
