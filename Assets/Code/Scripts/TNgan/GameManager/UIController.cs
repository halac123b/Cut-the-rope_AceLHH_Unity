using System;
using UnityEngine;
using UnityEngine.Serialization;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public StarUI StarUIComponent;
}
