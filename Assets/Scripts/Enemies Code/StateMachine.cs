using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StateMachine : MonoBehaviour
{
    //Clase base Entity puede ser Enemigo/Player/Fruta/ 
    public Enemy Enemy { get; private set; }

    //BaseState debe ser scriptableObject
    private BaseState currentState;
    [SerializeField]private List<BaseState> states = null;

    private void Awake()
    {
        Enemy = GetComponent<Enemy>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SwitchState(StateType.Patrol);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    private void FixedUpdate()
    {
        currentState.Collisions(this);
    }

    public void SwitchState(StateType type)
    {
        currentState = states.Find(state => state.type == type);
        currentState.EnterState(this);
    }

    private void OnDrawGizmos()
    {
        var area = Enemy != null ? Enemy.Sensor.radiusDetection : 5f;
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, area);
    }
}
