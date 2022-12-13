using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventListener : MonoBehaviour
{

    public GameEvent gameStartEvent;

    public Transform GOEffects;
    // Start is called before the first frame update
    private void Awake()
    {

    }

    private void OnEnable()
    {
        gameStartEvent.OnEventRaised += StartEffects;
    }

    private void OnDisable()
    {
        gameStartEvent.OnEventRaised -= StartEffects;
    }

    void StartEffects()
    {
        GOEffects.SetActiveAllChildrens();
    }

}
