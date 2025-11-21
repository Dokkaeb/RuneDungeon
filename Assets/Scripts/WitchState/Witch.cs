using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Witch : MonoBehaviour
{
    IWitchState _currentState;
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _jumpForce = 5f;
    bool _jumpInputPressed;
    Vector2 _moveInput;
    Rigidbody2D _rb;
    SpriteRenderer _spr;

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

    private void Awake()
    {
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
}
