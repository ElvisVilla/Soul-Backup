using UnityEngine;

public class PatrolState : BaseState
{
    Enemy enemy;

    public override void EnterState(StateMachine stateMachine)
    {
        enemy = stateMachine.enemy;
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        enemy.Movement.PerformMovement(enemy, Time.deltaTime);
        Collisions(stateMachine);
    }

    public override void Collisions(StateMachine stateMachine)
    {
        //stateMachine.enemy.Sensor.OnDetected(stateMachine.transform, () =>
        //stateMachine.SwitchState(stateMachine.combat));
        //var enemy = stateMachine.enemy;

        enemy.Sensor.UpdateScan(stateMachine.transform, () =>
        stateMachine.SwitchState(stateMachine.combat));
    }
}
