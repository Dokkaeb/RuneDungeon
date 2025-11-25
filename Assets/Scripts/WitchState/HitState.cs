using UnityEngine;

public class HitState : IWitchState
{
    Witch _witch;
    private Vector3 _hitPos;
    private float _hitEndTime;
    public HitState(Witch witch, Vector3 hitPos)
    {
        _witch = witch;
        _hitPos = hitPos;
    }
    public void Enter()
    {
        _hitEndTime = Time.time+_witch.HitDuration;
        _witch.Rb.sharedMaterial = _witch._noFriction;
        _witch.Animator.SetTrigger("HitTrigger");
        ApplyKnockbackForce();
    }

    public void Exit()
    {
        _witch.Rb.sharedMaterial = _witch._useFriction;
    }

    public void Update()
    {
        if (Time.time >= _hitEndTime)
        {
            // 시간이 지나면 Idle 상태로 복귀
            _witch.SetState(new IdleState(_witch));
            return;
        }
    }

    private void ApplyKnockbackForce()
    {
        Vector2 dir = (_witch.transform.position - _hitPos).normalized;
        dir.y = Mathf.Clamp(dir.y, 0.5f, 1f);

        _witch.Rb.linearVelocity = Vector2.zero;
        _witch.Rb.AddForce(dir * _witch.KnockbackForce, ForceMode2D.Impulse);
    }
}
