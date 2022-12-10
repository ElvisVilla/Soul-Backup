using UnityEngine;
using System;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "IA/State/Combat")]
public class CombatState : BaseState
{
    Enemy enemy;
    Sensor sensor;

    private void OnEnable()
    {
        type = StateType.Combat;
    }

    public override void EnterState(StateMachine stateMachine)
    {
        enemy = stateMachine.Enemy;
        sensor = enemy.Sensor;
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        var isDetecting = sensor.IsDetectingTarget(enemy.transform);
        if (isDetecting)
        {
            enemy.Movement.Move(enemy, Time.deltaTime, sensor.target);
        }
        else
        {
            stateMachine.SwitchState(StateType.Patrol);
        }
    }

    public override void Collisions(StateMachine stateMachine)
    {
        sensor.UpdateScan(enemy.transform);
    }
}
