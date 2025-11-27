using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Chase", story: "[Self] Chase [Player] with [MoveSpeed]", category: "Action", id: "aaa3828eea785ea5ed6dbf3b039a6052")]
public partial class ChaseAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<float> MoveSpeed;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Vector3 dir = (Player.Value.transform.position - Self.Value.transform.position).normalized;
        Self.Value.transform.position += dir * MoveSpeed.Value * Time.deltaTime;
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

