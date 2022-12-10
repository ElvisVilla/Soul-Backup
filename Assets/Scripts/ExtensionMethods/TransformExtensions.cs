using System;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    /// <summary>
    /// Se establece la referencia del componente una sola vez. Util cuando se necesita obtener una referencia en 
    /// MonoBehaviour.Update()
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="transform"></param>
    /// <param name="componentToSet"></param>
    public static void GetComponentOnce<T>(this Transform transform, ref T componentToSet)
    {
        if(componentToSet == null)
        {
            componentToSet = transform.GetComponent<T>();
            if (componentToSet == null)
                throw new Exception("No se encontro componente en este objeto");
        }
    }

    /// <summary>
    /// Retorna una lista con los transform hijos.
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static List<Transform> GetChildTransforms(this Transform transform)
    {
        List<Transform> items = new List<Transform>();

        if(transform.childCount == 0)
            return items;

        for (int i = 0; i < transform.childCount; i++)
            items.Add(transform.GetChild(i));

        return items;        
    }

    public static IEnumerable<Transform> GetTransforms(this Transform transform)
    {
        foreach (Transform item in transform)
        {
            yield return item;
        }
    }

    /// <summary>
    /// Convierte la lista de transforms hijos en una lista de componente especifico.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static List<T> GetChildElementsTo<T>(this Transform transform) where T : Component
    {
        List<Transform> elements = new List<Transform>();

        foreach (Transform item in transform)
        {
            elements.Add(item);
        }

        return elements.ConvertAll(new Converter<Transform, T>(item => item.GetComponent<T>()));   
    }

    /// <summary>
    /// Retorna un transform con el respectivo nombre del GameObject.
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Transform GetChildByName(this Transform transform, string name)
    {
        List<Transform> elements = transform.GetChildTransforms();

        for (int i = 0; i < elements.Count; i++)
        {
            if(elements[i].name == name){
                return elements[i];
            }
        }
        throw new System.Exception("No Transform was found");
    }

    public static void SetLocalPositionToZero(this Transform transform) => transform.localPosition = Vector3.zero;
}
