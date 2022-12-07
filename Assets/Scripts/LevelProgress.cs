using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgress : MonoBehaviour
{

    [SerializeField] Slider slider;
    [SerializeField] Transform player;
    [SerializeField] Transform endLine;
    [SerializeField] Text porcentageText;
    private float maxDistance;
    private float dist = 0;
    [SerializeField] float distanciaDebug;


    // Start is called before the first frame update
    void Start()
    {
        maxDistance = getDistance();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.position.x <= maxDistance && player.position.x <= endLine.position.x)
        {
            dist = 1 - (getDistance() / maxDistance);
            setProgress(dist);
        }
    }

    private float getDistance()
    {
        return Vector2.Distance(player.position, endLine.position);
    }

    private void setProgress(float value)
    {
        slider.value = value;
        var val = Mathf.Clamp(value * 100f, 0f, 101f);
        porcentageText.text = Mathf.Floor(val).ToString();
    }

}
