using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Witch : MonoBehaviour
{
    IWitchState _currentState;
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _jumpForce = 5f;
    bool _jumpInputPressed;
    bool _interactInputPressed;
    Vector2 _moveInput;
    Rigidbody2D _rb;
    SpriteRenderer _spr;
    Animator _animator;
    IInteractable _currentInteractable;

    [SerializeField] LayerMask _groundLayer;
    [SerializeField] Transform _groundChecker;
    [SerializeField] float _groundCheckDistance = 0.1f;
    bool _isGrounded;

    public PhysicsMaterial2D _noFriction;
    public PhysicsMaterial2D _useFriction;

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
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _spr = GetComponent<SpriteRenderer>();
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
}
