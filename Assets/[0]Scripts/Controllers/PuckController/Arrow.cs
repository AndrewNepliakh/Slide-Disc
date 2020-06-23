using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour, IPoolable
{
    public void OnActivate(object argument = default)
    {
        gameObject.SetActive(true);
    }

    public void OnDeactivate(object argument = default)
    {
        gameObject.SetActive(false);
    }
}
