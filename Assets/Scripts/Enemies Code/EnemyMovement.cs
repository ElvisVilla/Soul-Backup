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

    //Modificar PerformMovement de modo que pueda cambiar target para tambien seguir al jugador.
    //Va a seguir al al target siempre.

    public void PerformMovement(Enemy enemy, float delta)
    {
        distanceToWaypoint = Vector2.Distance(enemy.transform.position, wayPoints[indexWaypoint].position);
        //Animations();

        if (distanceToWaypoint > 0.5f)
        {
            enemy.Body.position = Vector2.MoveTowards(enemy.Body.position, wayPoints[indexWaypoint].position,
            speed * Time.deltaTime);
            //Animations();
        }
        else
        {
            if (doOnce == false)
            {
                waitTime = Random.Range(1f, 5f);
                enemy.StartCoroutine(Wait());
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

    public void Move(Enemy enemy, float delta, Transform target)
    {
        var targetDirection = target.position - enemy.transform.position;
        var distance = Vector2.Distance(target.position, enemy.transform.position);
        Debug.Log(distance);

        if (distance > stoppingDistance)
        {
            enemy.Body.position = Vector2.MoveTowards(enemy.Body.position, target.transform.position,
            followSpeed * Time.deltaTime);
        }
    }

    //Una demostracion de la clase Sensor.
    //if (sensor.IsDetected())
    //{
    //    print("Detected with bool");
    //}

    //sensor.OnDetected(() => print("Detecting with Ondetected"));

    //void Animations()
    //{
    //    anim.SetBool("IsMoving", distanceToWaypoint > 0.55f);
    //    if(body.position.x > wayPoints[IndexWaypoint].position.x)
    //    {
    //        sprite.flipX = true;
    //    }
    //    else
    //    {
    //        sprite.flipX = false;
    //    }
}