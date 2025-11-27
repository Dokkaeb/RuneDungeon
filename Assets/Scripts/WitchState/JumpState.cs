using UnityEngine;



public class JumpState : IWitchState
{
    private Witch _witch;
    public JumpState(Witch witch)
    {
        _witch = witch;
    }

    public void Enter()
    {
        Vector2 velocity = _witch.Rb.linearVelocity;
        velocity.y = _witch.JumpForce;
        _witch.Rb.linearVelocity = velocity;

        //점프 수행중에는 마찰력을 없애서 벽에 붙는거 방지
        _witch.Rb.sharedMaterial = _witch._noFriction;

        _witch.Animator.SetBool("IsGrounded", false);
    }

    public void Exit()
    {
        //점프가 끝나면 다시 마찰력 사용
        _witch.Rb.sharedMaterial = _witch._useFriction;
    }

    public void Update()
    {
        //점프중 공중이동
        Vector2 moveInput = _witch.MoveInput;
        Vector2 velocity = _witch.Rb.linearVelocity;
        velocity.x = moveInput.x * _witch.MoveSpeed;
        _witch.Rb.linearVelocity = velocity;

        if (moveInput.x > 0)
        {
            _witch.Spr.flipX = false;
        }
        else if (moveInput.x < 0)
        {
            _witch.Spr.flipX = true;
        }

        if (_witch.IsGrounded && velocity.y <= 0.01f)
        {
            _witch.Animator.SetBool("IsGrounded", true);

            //입력값있으면 무브,아니면 대기상태로 변경
            if (_witch.MoveInput.x != 0)
            {
                _witch.SetState(new MoveState(_witch));
            }
            else
            {
                _witch.SetState(new IdleState(_witch));
            }
        }


    }
}
