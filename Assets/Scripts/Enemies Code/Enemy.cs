using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(StateMachine))]
public class Enemy : MonoBehaviour
{
    //[HideInInspector] private Rigidbody2D _body;
    [HideInInspector] private Animator _anim;
    [HideInInspector] private SpriteRenderer _sprite;
    public Rigidbody2D Body { get; private set; }


    [SerializeField] private Sensor _sensor = new Sensor();
    public Sensor Sensor => _sensor;



    [Tooltip("Movement is suppose to be changeable with other type of movement.")]
    [Header("Movement Data: ")]
    [SerializeField] private EnemyMovement _movement = new EnemyMovement();
    public EnemyMovement Movement => _movement;


    // Start is called before the first frame update
    void Awake()
    {
        _anim = GetComponent<Animator>();
        Body = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        //_sensor = GetComponentInChildren<Sensor>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
