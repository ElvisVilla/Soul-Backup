using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class EnemyMovement
{
    [SerializeField] private float clossestDistance = -2f;
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

    public void WayPointMovement(Enemy owner)
    {
        distanceToWaypoint = Vector2.Distance(owner.transform.position, wayPoints[indexWaypoint].position);

        if (distanceToWaypoint > 0.5f)
        {
            owner.Animator.SetBool("IsMoving", true);
            owner.Body.position = Vector2.MoveTowards(owner.Body.position, wayPoints[indexWaypoint].position,
            speed * Time.deltaTime);
        }
        else
        {
            owner.Animator.SetBool("IsMoving", false);
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

    public void Move(Enemy owner, Transform target, float speed = 1f)
    {

        var targetDirection = target.position - owner.transform.position;
        var distance = Vector2.Distance(target.position, owner.transform.position);

        if (distance > stoppingDistance)
        {
            owner.Animator.SetBool("IsMoving", true);
            owner.Body.position = Vector2.MoveTowards(owner.Body.position, target.transform.position,
            followSpeed * Time.deltaTime);
        }
        else if(distance < stoppingDistance -1.5f)
        {
            owner.Animator.SetBool("IsMoving", true);
            owner.Body.position = Vector2.MoveTowards(owner.Body.position, target.transform.position,
            followSpeed * speed * Time.deltaTime);
        }
        else
        {
            owner.Animator.SetBool("IsMoving", false);
        }
    }

    void Animations(Enemy owner)
    {
       
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