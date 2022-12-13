using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OneTime
{
    bool performedOne;

    public void PerformOne(Action action)
    {
        if (!performedOne)
        {
            action?.Invoke();
            performedOne = true;
        }
    }

    public void Reestart()
    {
        performedOne = false;
    }
}
