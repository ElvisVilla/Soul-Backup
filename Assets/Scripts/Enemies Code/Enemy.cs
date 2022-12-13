using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(StateMachine))]
public class Enemy : MonoBehaviour
{
    //[HideInInspector] private Rigidbody2D _body;
    public Animator Animator { get; private set; }
    public SpriteRenderer Sprite { get; private set; }
    public Rigidbody2D Body { get; private set; }

    [field:SerializeField]public Sensor Sensor { get; private set; } = new Sensor();

    [field:Space]
    [field: SerializeField] public EnemyMovement Movement { get; private set; } = new EnemyMovement();

    // Start is called before the first frame update
    void Awake()
    {
        Animator = GetComponent<Animator>();
        Body = GetComponent<Rigidbody2D>();
        Sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(7, 6);

    }
}
