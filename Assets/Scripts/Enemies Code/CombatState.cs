using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class CombatState : BaseState
{
    Enemy enemy;

    public override void EnterState(StateMachine stateMachine)
    {
        enemy = stateMachine.enemy;
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        var sensor = enemy.Sensor;

        if (sensor.IsDetected(enemy.transform))
        {
            enemy.Movement.Move(enemy, Time.deltaTime, sensor.target);
        }
        else
        {
            stateMachine.SwitchState(stateMachine.patrol);
        }
    }

    public override void Collisions(StateMachine stateMachine)
    {
        stateMachine.enemy.Sensor.UpdateScan(enemy.transform);
    }
}
