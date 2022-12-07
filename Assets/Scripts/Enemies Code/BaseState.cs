using UnityEngine;

public abstract class BaseState
{
    public abstract void EnterState(StateMachine stateMachine);
    public abstract void UpdateState(StateMachine stateMachine);
    public abstract void Collisions(StateMachine stateMachine);
}
