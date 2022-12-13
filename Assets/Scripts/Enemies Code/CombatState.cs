using UnityEngine;
using System;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "IA/State/Combat")]
public class CombatState : BaseState
{


    private void OnEnable()
    {
        type = StateType.Combat;
    }

    public override void EnterState(StateMachine stateMachine)
    {
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        var isDetecting = stateMachine.Enemy.Sensor.IsDetectingTarget(stateMachine.Enemy.transform);
        if (isDetecting)
        {
            stateMachine.Enemy.Movement.Move(stateMachine.Enemy, stateMachine.Enemy.Sensor.target, -1);
        }
        else
        {
            stateMachine.SwitchState(StateType.Patrol);
        }
    }

    public override void Collisions(StateMachine stateMachine)
    {
        stateMachine.Enemy.Sensor.UpdateScan(stateMachine.Enemy.transform);
    }
}
