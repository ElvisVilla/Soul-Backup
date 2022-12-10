using UnityEngine;

public enum StateType
{
    Patrol,
    Combat,
    Chase,
    Dead,
}

public abstract class BaseState: ScriptableObject
{
    public StateType type { get; protected set; }

    public abstract void EnterState(StateMachine stateMachine);
    public abstract void UpdateState(StateMachine stateMachine);
    public abstract void Collisions(StateMachine stateMachine);
}
