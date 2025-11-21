using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;


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
    }
}
