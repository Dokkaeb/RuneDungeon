using UnityEngine;



public class IdleState : IWitchState
{
    Witch _witch;
    public IdleState(Witch witch)
    {
        _witch = witch;
    }

    public void Enter()
    {
        Vector2 velocity = _witch.Rb.linearVelocity;
        velocity.x = 0f;
        _witch.Rb.linearVelocity = velocity;

        _witch.Rb.sharedMaterial = _witch._useFriction;

        _witch.Animator.SetBool("IsMove", false);
        _witch.Animator.SetBool("IsGrounded", true);
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
        if(_witch.MoveInput.x != 0)
        {
            _witch.SetState(new MoveState(_witch));
            return;
        }

        if (_witch.InteractInputPressed)
        {
            _witch.SetState(new InteractState(_witch));
            return;
        }
    }
}
