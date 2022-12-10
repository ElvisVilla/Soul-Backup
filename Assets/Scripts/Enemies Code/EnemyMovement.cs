using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class EnemyMovement
{
    [SerializeField] private float stoppingDistance = 2f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private int indexWaypoint = 0;

    Rigidbody2D body;
    bool doOnce = false;
    float distanceToWaypoint;

    public Transform target;

    public void Init(Enemy enemy)
    {
        body = enemy.Body;
    }

    public void WayPointMovement(Enemy owner)
    {
        distanceToWaypoint = Vector2.Distance(owner.transform.position, wayPoints[indexWaypoint].position);

        if (distanceToWaypoint > 0.5f)
        {
            owner.Body.position = Vector2.MoveTowards(owner.Body.position, wayPoints[indexWaypoint].position,
            speed * Time.deltaTime);
            //Animations();
        }
        else
        {
            if (doOnce == false)
            {
                waitTime = Random.Range(1f, 5f);
                owner.StartCoroutine(Wait());
                doOnce = true;
            }
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTime);

        if (indexWaypoint < wayPoints.Length - 1)
            indexWaypoint++;
        else
            indexWaypoint = Random.Range(0, 2);

        doOnce = false;
    }

    public void Move(Enemy owner, float delta, Transform target)
    {
        var targetDirection = target.position - owner.transform.position;
        var distance = Vector2.Distance(target.position, owner.transform.position);
        Debug.Log(distance);

        if (distance > stoppingDistance)
        {
            
            owner.Body.position = Vector2.MoveTowards(owner.Body.position, target.transform.position,
            followSpeed * Time.deltaTime);
        }
    }

    void Animations(Enemy owner)
    {
        owner.Animator.SetBool("IsMoving", distanceToWaypoint > 0.5f);
    }

    void SpriteOrientation(Enemy owner)
    {
        if (body.position.x > wayPoints[indexWaypoint].position.x)
        {
            owner.Sprite.flipX = true;
        }
        else
        {
            owner.Sprite.flipX = false;
        }
    }
}