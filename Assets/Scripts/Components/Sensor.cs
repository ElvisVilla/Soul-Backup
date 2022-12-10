using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Sensor
{
    public float radiusDetection = 5f;
    [SerializeField] LayerMask maskDetection;
    private Collider2D[] actors;
    private bool _isDetected;

    public Transform target;
    
    public bool IsDetectingTarget(Transform owner)
    {
        return _isDetected;
    }        

    //Update the Sensor and also can trigger methods when something is detected.
    public void UpdateScan(Transform owner, Action OnSomethingDetected = null)
    {
        var coll = Scan(owner);

        if (coll != null)
        {
            _isDetected = true;
            target = coll.transform; //Ontiene la referencia de target una sola vez.
            OnSomethingDetected?.Invoke();
        }
        else
        {
            target = null;
            _isDetected = false;
        }
    }

    public Collider2D Scan(Transform owner)
    {
        return Physics2D.OverlapCircle(owner.position, radiusDetection, maskDetection);
    }

    public void OnDetected(Transform owner ,Action whatHapenOnceDetected = null)
    {
        actors = ScanAllColliders(owner);
        IterateColliders(actors, whatHapenOnceDetected);        
    }

    private Collider2D[] ScanAllColliders(Transform owner)
    {
        return actors = Physics2D.OverlapCircleAll(owner.position, radiusDetection, maskDetection);
    }

    private void IterateColliders(Collider2D[] areas, Action logic = null)
    {
        foreach (var actor in actors)
        {
            if (actor != null)
            {
                logic?.Invoke();
                //Deberia actor.DoSomething().
            }
        }
    }
}
