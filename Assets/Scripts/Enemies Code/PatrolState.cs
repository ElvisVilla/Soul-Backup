using UnityEngine;

[CreateAssetMenu(menuName = "IA/State/Patrol")]
public class PatrolState : BaseState
{
    //Has to derive from a base class.

    private void OnEnable()
    {
        type = StateType.Patrol;
    }

    public override void EnterState(StateMachine stateMachine)
    {
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        stateMachine.Enemy.Movement.WayPointMovement(stateMachine.Enemy);
    }

    public override void Collisions(StateMachine stateMachine)
    {
        stateMachine.Enemy.Sensor.UpdateScan(stateMachine.transform, () =>
        stateMachine.SwitchState(StateType.Combat));
    }
}
