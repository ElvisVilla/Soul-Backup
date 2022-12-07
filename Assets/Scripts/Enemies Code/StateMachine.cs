using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StateMachine : MonoBehaviour
{

    //Clase base Entity puede ser Enemigo/Player/Fruta/ 
    [HideInInspector] public Enemy enemy;

    //BaseState debe ser scriptableObject
    //Debe haber una lista de estados asignables como scriptableObjects.
    public BaseState currentState;
    public PatrolState patrol = new PatrolState();
    public CombatState combat = new CombatState();
    public HurtState hurt = new HurtState();
    public DeadState dead = new DeadState();

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = patrol;
        currentState.EnterState(this);
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

    public void SwitchState(BaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    private void OnDrawGizmos()
    {
        
        var area = enemy != null ? enemy.Sensor.radiusDetection : 5f;
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, area);

    }
}
