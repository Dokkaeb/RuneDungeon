using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Detect", story: "[Self] Detect [Player] with [DetectRange]", category: "Action", id: "43a89924621413e62aacac64b79d5d83")]
public partial class DetectAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<float> DetectRange;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        float distance = Vector2.Distance(Self.Value.transform.position, Player.Value.transform.position);
        return distance <= DetectRange.Value ? Status.Success : Status.Failure;
    }

    protected override void OnEnd()
    {
    }
}

