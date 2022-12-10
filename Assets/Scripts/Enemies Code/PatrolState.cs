using UnityEngine;

[CreateAssetMenu(menuName = "IA/State/Patrol")]
public class PatrolState : BaseState
{
    //Has to derive from a base class.
    Enemy enemy;
    Sensor sensor;

    

    private void OnEnable()
    {
        type = StateType.Patrol;
    }

    public override void EnterState(StateMachine stateMachine)
    {
        enemy = stateMachine.Enemy;
        sensor = enemy.Sensor;
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        enemy.Movement.WayPointMovement(enemy);
    }

    public override void Collisions(StateMachine stateMachine)
    {
        sensor.UpdateScan(stateMachine.transform, () =>
        stateMachine.SwitchState(StateType.Combat));
    }
}
