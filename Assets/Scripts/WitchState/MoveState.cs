using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MoveState : IWitchState
{
    private Witch _witch;
    private float _moveSpeed;
    public MoveState(Witch witch)
    {
        _witch = witch;
        _moveSpeed = witch.MoveSpeed;
    }
    public void Enter()
    {
        //¸¶Âû·Â »ç¿ë
        _witch.Rb.sharedMaterial = _witch._useFriction;
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        if(_witch.IsGrounded && _witch.JumpInputPressed)
        {
            _witch.SetState(new JumpState(_witch));
            return;
        }
        if (_witch.MoveInput.x == 0)
        {
            _witch.SetState(new IdleState(_witch));
            return;
        }


        Vector2 _moveInput = _witch.MoveInput;
        if (_moveInput.x > 0)
        {
            _witch.Spr.flipX = false;
        }
        else if( _moveInput.x < 0)
        {
            _witch.Spr.flipX= true;
        }
        Vector2 _velocity = _witch.Rb.linearVelocity;
        _velocity.x = _moveInput.x*_moveSpeed;
        _witch.Rb.linearVelocity = _velocity;
    }
}
