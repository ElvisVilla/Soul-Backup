using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorExtensions
{

    /// <summary>
    /// Selecciona en base al tipo de valor que se pase en el metodo. Reemplaza SetBool, SetFloat y SetInt. 
    /// Internamiente convierte el string a int para conservar rendimiento.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="animator"></param>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public static void PerformAnimation<T>(this Animator animator, string name, T value)
    {
        int hashCode = Animator.StringToHash(name);

        if (value is bool boolValue)
        {
            animator.SetBool(hashCode, boolValue);
        }
        else if (value is float floatValue)
        {
            animator.SetFloat(hashCode, floatValue);
        }
        else if (value is int intValue)
        {
            animator.SetInteger(hashCode, intValue);
        }
    }

    public static void PerformCrossFade(this Animator animator, string name, float secondsToFade)
    {
        int hashCode = Animator.StringToHash(name);
        animator.CrossFade(hashCode, secondsToFade);
    }

    public static void PerformTriggerAnimation(this Animator animator, string name)
    {
        int hashCode = Animator.StringToHash(name);
        animator.SetTrigger(hashCode);
    }
}
