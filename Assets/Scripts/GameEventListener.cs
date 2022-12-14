using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventListener : MonoBehaviour
{

    public GameEvent gameStartEvent;

    public List<Health> effects;
    // Start is called before the first frame update
    private void Awake()
    {
        effects = transform.GetChildElementsTo<Health>();
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
        foreach (var effect in effects)
        {
            effect.gameObject.SetActive(true);
        }
    }

}
