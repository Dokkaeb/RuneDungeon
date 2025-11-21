using UnityEngine;

public class InteractState : IWitchState
{
    private Witch _witch;
    public InteractState(Witch witch)
    {
        _witch = witch;
    }
    public void Enter()
    {
        _witch.TryInteract();
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        _witch.SetState(new IdleState(_witch));
    }
}
