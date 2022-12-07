using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaximizeWindows : MonoBehaviour
{
    public bool maximize = false;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (maximize)
        {
            UnityEditor.EditorWindow.focusedWindow.maximized = !UnityEditor.EditorWindow.focusedWindow.maximized;
        }
#endif
    }
}
