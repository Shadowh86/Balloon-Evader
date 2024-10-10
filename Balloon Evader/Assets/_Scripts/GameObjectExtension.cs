using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// GameObject extension to unity game objects
/// @Tomislav Marković
/// </summary>
public static class GameObjectExtension
{
    /// <summary>
    /// gameObject.SetActive(true)
    /// </summary>
    public static void Activate(this GameObject obj)
    {
        obj.SetActive(true);
    }

    /// <summary>
    /// gameObject.SetActive(false)
    /// </summary>
    public static void Deactivate(this GameObject obj)
    {
        obj.SetActive(false);
    }
    
    public static void ResetTransform(this Transform tran)
    {
        tran.position = Vector3.zero;
        tran.rotation = Quaternion.identity;
        tran.localScale = new Vector3(1, 1, 1);
    }
}